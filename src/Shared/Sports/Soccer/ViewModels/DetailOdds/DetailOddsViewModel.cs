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
            this.matchId = matchId;
            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(BetType.OneXTwo, true), false));
        }

        public ObservableCollection<BaseItemViewModel> BetTypeOdds { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public bool NoData { get; private set; }

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
            var odds = await oddsService.GetOdds(matchId, (int)betType, isRefresh);

            if (odds.BetTypeOddsList?.Any() == true)
            {
                BetTypeOdds = new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                   new BaseItemViewModel(betType, t, NavigationService, DependencyResolver)
                   .CreateInstance()));

                HeaderTemplate = new BaseHeaderViewModel(betType, NavigationService, DependencyResolver).CreateTemplate();

                HasData = true;
                NoData = !HasData;
            }
            else
            {
                HasData = false;
                NoData = !HasData;
            }

            IsRefreshing = false;
        }
    }
}