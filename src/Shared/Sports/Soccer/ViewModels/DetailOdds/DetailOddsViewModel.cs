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
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModelBase
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            //this.matchId = matchId;
            this.matchId = "sr:match:18575332";
            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, true), false));

            LoadOneXTwoCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.OneXTwo), false));
            LoadAsianHdpCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.AsianHDP), false));
            LoadOverUnderCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.OverUnder), false));
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

        protected override async void Initialize()
        {
            try
            {
                await LoadData(() => LoadOdds(BetType.OneXTwo));
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadOdds(BetType betType, bool isRefresh = false)
        {
            SelectedBetType = betType;

            var odds = await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, (int)betType, isRefresh);

            HasData = odds.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(betType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            BetTypeOdds = HasData
                ? new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                    new BaseItemViewModel(betType, t, NavigationService, DependencyResolver)
                    .CreateInstance()))
                : new ObservableCollection<BaseItemViewModel>();

            IsRefreshing = false;
        }
    }
}