using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Common.Configuration;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModelBase
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private readonly string matchId;
        private readonly string oddsFormat;

        private readonly IOddsService oddsService;
        private HubConnection hubConnection;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            this.matchId = matchId;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;



            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(HandleButtonCommand);

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);

            TabHeaderIcon = TabDetailImages.Odds;
            TabHeaderActiveIcon = TabDetailImages.OddsActive;
        }

        public ObservableCollection<BaseItemViewModel> BetTypeOdds { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<string> OnOddsTabClicked { get; }

        public DelegateAsyncCommand<BaseItemViewModel> TappedOddsItemCommand { get; }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public BetType SelectedBetType { get; private set; }

        public bool IsOneXTwoSelected => SelectedBetType == BetType.OneXTwo;

        public bool IsAsianHdpSelected => SelectedBetType == BetType.AsianHDP;

        public bool IsOverUnderSelected => SelectedBetType == BetType.OverUnder;

        protected override async void Initialize()
        {
            try
            {
                await LoadData(() => LoadOdds(SelectedBetType, oddsFormat));

                //hubConnection = DependencyResolver
                    //.Resolve<IHubService>(SettingsService.CurrentSportType.Value.ToString())
                    //.BuildOddsEventHubConnection();

                hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Configuration.LocalHubEndPoint}Soccer/OddsEventHub")
                .Build();

                hubConnection.On("OddsComparison", (Action<byte, string>)((sportId, data) =>
                {
                    var oddsComparisonMessage = JsonConvert.DeserializeObject<MatchOddsComparisonMessage>(data);

                    UpdateOdds(oddsComparisonMessage);

                }));

                hubConnection.Closed += async (error) =>
                {
                    await LoggingService.LogErrorAsync(error);

                    await StartOddsHubConnection();
                };

                await StartOddsHubConnection();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private void UpdateOdds(MatchOddsComparisonMessage oddsComparisonMessage)
        {
            if (oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase) &&
                oddsComparisonMessage.BetTypeOddsList != null &&
                oddsComparisonMessage.BetTypeOddsList.Any(x => x.Id == (int)SelectedBetType))
            {
                var betTypeOdds = oddsComparisonMessage.BetTypeOddsList.Where(x => x.Id == (int)SelectedBetType);

                var updatedOddsViewModels = new ObservableCollection<BaseItemViewModel>(betTypeOdds.Select(t =>
                   new BaseItemViewModel(SelectedBetType, t, NavigationService, DependencyResolver)
                   .CreateInstance()));

                foreach (var updatedOdds in updatedOddsViewModels)
                {
                    if (BetTypeOdds.Any(x => x.Bookmaker.Equals(updatedOdds.Bookmaker)))
                    {
                        var viewModel = BetTypeOdds.FirstOrDefault(x => x.Bookmaker.Equals(updatedOdds.Bookmaker));

                        viewModel.UpdateOdds(updatedOdds.BetTypeOdds);
                    }
                    else
                    {
                        HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, true, NavigationService, DependencyResolver).CreateTemplate();
                        BetTypeOdds.Add(updatedOdds);
                    }
                }

            }
        }

        private async Task StartOddsHubConnection()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            await hubConnection.StartAsync(cancellationTokenSource.Token);
        }

        private Task HandleButtonCommand(string betTypeId)
            => LoadData(() => LoadOdds((BetType)(int.Parse(betTypeId)), oddsFormat));

        private async Task LoadOdds(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsLoading = !isRefresh;

                SelectedBetType = betType;

                var odds = await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, (int)betType, formatType, isRefresh);

                HasData = odds.BetTypeOddsList?.Any() == true;

                HeaderTemplate = new BaseHeaderViewModel(betType, HasData, NavigationService, DependencyResolver).CreateTemplate();

                BetTypeOdds = HasData
                    ? new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                        new BaseItemViewModel(betType, t, NavigationService, DependencyResolver)
                        .CreateInstance()))
                    : new ObservableCollection<BaseItemViewModel>();

                IsRefreshing = false;
                IsLoading = false;
            }
        }

        private bool CanLoadOdds(BetType betType, bool isRefresh)
            => isRefresh || SelectedBetType != betType || BetTypeOdds == null || !BetTypeOdds.Any();

        private async Task HandleOddsItemTapCommand(BaseItemViewModel item)
        {
            var parameters = new NavigationParameters
            {
                { "MatchId", matchId },
                { "Bookmaker", item.BetTypeOdds.Bookmaker},
                { "BetType", SelectedBetType },
                { "Format",  oddsFormat}
            };

            var navigated = await NavigationService.NavigateAsync("OddsMovementView" + SettingsService.CurrentSportType.Value, parameters);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception);
            }
        }
    }
}