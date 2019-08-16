using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class OddsMovementViewModel : ViewModelBase
    {


        private readonly IOddsService oddsService;

        private string matchId;
        private Bookmaker bookmaker;
        private BetType betType;
        private string oddsFormat;

        public OddsMovementViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOddsMovement(true)));
        }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public bool HasNoData => !HasData;

        public ObservableCollection<IGrouping<dynamic, BaseMovementItemViewModel>> GroupOddsMovementItems { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DataTemplate HeaderTemplate { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            try
            {
                matchId = parameters["MatchId"].ToString();
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds";
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);
            }
        }

        protected override async void Initialize()
        {
            try
            {
                await LoadData(() => LoadOddsMovement());
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        [Time]
        private async Task LoadOddsMovement(bool isRefresh = false)
        {
            IsLoading = !isRefresh;

            var matchOddsMovement = await oddsService.GetOddsMovement(SettingsService.CurrentLanguage, matchId, (int)betType, oddsFormat, bookmaker.Id, isRefresh);

            HasData = matchOddsMovement.OddsMovements?.Any() == true;

            HeaderTemplate = new BaseMovementHeaderViewModel(betType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            var OddsMovementItems = HasData
                    ? new ObservableCollection<BaseMovementItemViewModel>(matchOddsMovement.OddsMovements.Select(t =>
                        new BaseMovementItemViewModel(betType, t, NavigationService, DependencyResolver)
                        .CreateInstance()))
                    : new ObservableCollection<BaseMovementItemViewModel>();

            GroupOddsMovementItems = HasData
                ? new ObservableCollection<IGrouping<dynamic, BaseMovementItemViewModel>>(OddsMovementItems.GroupBy(item
                    => new { item.CurrentSportName }))
                : new ObservableCollection<IGrouping<dynamic, BaseMovementItemViewModel>>();

            IsRefreshing = false;
            IsLoading = false;
        }
    }
}