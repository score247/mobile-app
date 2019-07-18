using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModelBase
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;
        private readonly string oddsFormat;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            this.matchId = "sr:match:18575332";

            //this.matchId = matchId;

            oddsFormat = OddsFormat.Decimal.Value;
            SelectedBetType = BetType.OneXTwo;

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(HandleButtonCommand);

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);
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
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private Task HandleButtonCommand(string betTypeId)
        => LoadData(() => LoadOdds((BetType)(int.Parse(betTypeId)), oddsFormat));    

        private async Task LoadOdds(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsLoading = true;

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
                { "Bettype", SelectedBetType },
                { "Format",  oddsFormat}
            };

            await NavigationService.NavigateAsync("OddsMovementView" + SettingsService.CurrentSportType.Value, parameters);
        }
    }
}