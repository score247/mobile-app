using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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

    internal class DetailOddsViewModel : TabItemViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private readonly string matchId;
        private readonly string oddsFormat;

        private readonly IOddsService oddsService;
        private readonly HubConnection hubConnection;

        private bool disposedValue;
        private CancellationTokenSource cancellationTokenSource;

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

            hubConnection = DependencyResolver
                .Resolve<IHubService>(CurrentSportId.ToString())
                .BuildOddsEventHubConnection();

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(HandleButtonCommand);

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);

            TabHeaderIcon = TabDetailImages.Odds;
            TabHeaderActiveIcon = TabDetailImages.OddsActive;
        }

        public ObservableCollection<BaseItemViewModel> BetTypeOddsItems { get; private set; }

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

        private Task HandleButtonCommand(string betTypeId)
            => LoadData(() => LoadOdds((BetType)(int.Parse(betTypeId)), oddsFormat));

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

        protected override async void Initialize()
        {
            try
            {
                await LoadData(() => LoadOdds(SelectedBetType, oddsFormat));

                hubConnection.On("OddsComparison", (Action<byte, string>)(async (sportId, data) =>
                {
                    var oddsComparisonMessage = JsonConvert.DeserializeObject<MatchOddsComparisonMessage>(data);

                    await HandleOddsComparisonMessage(oddsComparisonMessage);
                }));

                await StartOddsHubConnection();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        protected override void Clean()
        {
            cancellationTokenSource?.Dispose();

            base.Clean();
        }

        public override void OnResume()
        {
            IsRefreshing = true;
            base.OnResume();
        }

        private async Task StartOddsHubConnection()
        {
            cancellationTokenSource = new CancellationTokenSource();

            await hubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token);
        }

        private async Task LoadOdds(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsLoading = !isRefresh;

                SelectedBetType = betType;

                var odds = await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, (int)betType, formatType, isRefresh);

                HasData = odds.BetTypeOddsList?.Any() == true;

                HeaderTemplate = new BaseHeaderViewModel(betType, HasData, NavigationService, DependencyResolver).CreateTemplate();

                BetTypeOddsItems = HasData
                    ? new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                        new BaseItemViewModel(betType, t, NavigationService, DependencyResolver)
                        .CreateInstance()))
                    : new ObservableCollection<BaseItemViewModel>();

                IsRefreshing = false;
                IsLoading = false;
            }
        }

        private bool CanLoadOdds(BetType betType, bool isRefresh)
            => isRefresh || SelectedBetType != betType || BetTypeOddsItems == null || !BetTypeOddsItems.Any();

        private async Task HandleOddsComparisonMessage(MatchOddsComparisonMessage oddsComparisonMessage)
        {
            //support only for 1X2 bet type at the moment
            if (SelectedBetType != BetType.OneXTwo)
            {
                return;
            }

            if (oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase) &&
                oddsComparisonMessage.BetTypeOddsList != null &&
                oddsComparisonMessage.BetTypeOddsList.Any(x => x.Id == (int)SelectedBetType))
            {
                var needToReOrder = false;
                var updatedBetTypeOdds = oddsComparisonMessage.BetTypeOddsList.Where(x => x.Id == (int)SelectedBetType);

                var updatedOddsViewModels = new ObservableCollection<BaseItemViewModel>(updatedBetTypeOdds.Select(t =>
                   new BaseItemViewModel(SelectedBetType, t, NavigationService, DependencyResolver)
                   .CreateInstance()));

                foreach (var updatedOdds in updatedOddsViewModels)
                {
                    var existingOddsItem = BetTypeOddsItems.FirstOrDefault(x => x.Bookmaker.Equals(updatedOdds.Bookmaker));

                    if (existingOddsItem == null)
                    {
                        needToReOrder = true;
                        AddBookmakerOdds(updatedOdds);
                    }
                    else
                    {
                        existingOddsItem.UpdateOdds(updatedOdds.BetTypeOdds);
                    }
                }

                if (needToReOrder)
                {
                    BetTypeOddsItems = new ObservableCollection<BaseItemViewModel>(BetTypeOddsItems.OrderBy(x => x.BetTypeOdds.Bookmaker.Name));
                }

                await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, (int)SelectedBetType, oddsFormat, forceFetchNewData: true);
            }
        }

        private void AddBookmakerOdds(BaseItemViewModel updatedOdds)
        {
            if (!BetTypeOddsItems.Any())
            {
                HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, true, NavigationService, DependencyResolver).CreateTemplate();
            }

            BetTypeOddsItems.Add(updatedOdds);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}