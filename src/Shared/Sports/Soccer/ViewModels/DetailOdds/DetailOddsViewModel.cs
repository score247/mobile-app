using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.Generic;
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
            this.matchId = matchId;

            oddsFormat = OddsFormat.Decimal.Value;
            SelectedBetType = BetType.OneXTwo;

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true), false));

            LoadOneXTwoCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.OneXTwo, oddsFormat), false));
            LoadAsianHdpCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.AsianHDP, oddsFormat), false));
            LoadOverUnderCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.OverUnder, oddsFormat), false));
        }

        public ObservableCollection<BaseItemViewModel> BetTypeOdds { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand LoadOneXTwoCommand { get; }

        public DelegateAsyncCommand LoadAsianHdpCommand { get; }

        public DelegateAsyncCommand LoadOverUnderCommand { get; }

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

        private async Task LoadOdds(BetType betType, string formatType, bool isRefresh = false)
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
}