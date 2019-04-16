namespace LiveScore.Score.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Constants;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Views;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private DateRange selectedDateRange;
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService,
            IEventAggregator eventAggregator)
            : base(navigationService, globalFactory, settingsService)
        {
            EventAggregator = eventAggregator;

            SetupDateBarSubscribers();

            SetupCommands();
        }

        public bool IsNotLoading { get; set; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchData { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> SelectMatchCommand { get; private set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || MatchData == null)
            {
                matchService = GlobalFactoryProvider
                   .ServiceFactoryProvider
                   .GetInstance((SportType)SettingsService.CurrentSportId)
                   .CreateMatchService();

                await LoadData(selectedDateRange);
            }
        }

        private void SetupDateBarSubscribers()
        {
            EventAggregator
                .GetEvent<HomeSelectedEvent>()
                .Subscribe(async () => await LoadDataFromYesterdayUntilNow());

            EventAggregator
                .GetEvent<DateSelectedEvent>()
                .Subscribe(async (selectedDate) => await LoadData(new DateRange(selectedDate)));
        }

        private void SetupCommands()
        {
            SelectMatchCommand = new DelegateAsyncCommand<IMatch>(NavigateToMatchDetailView);
            RefreshCommand = new DelegateAsyncCommand(async() => await LoadData(selectedDateRange));
        }

        private async Task NavigateToMatchDetailView(IMatch match)
            => await NavigationService.NavigateAsync(nameof(MatchDetailView), new NavigationParameters
            {
                { nameof(IMatch), match }
            });

        private async Task LoadData(
            DateRange dateRange,
            bool showLoadingIndicator = true,
            bool forceFetchNewData = false,
            bool isRefreshing = false)
        {
            IsNotLoading = !showLoadingIndicator;

            var matchData = await matchService.GetMatches(
                    SettingsService.CurrentSportId,
                    SettingsService.CurrentLanguage,
                    dateRange ?? DateRange.FromYesterdayUntilNow(),
                    forceFetchNewData);

            MatchData = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matchData.GroupBy(match
                      => new { match.League.Name, match.EventDate.Day, match.EventDate.Month, match.EventDate.Year }));

            selectedDateRange = dateRange;
            IsNotLoading = true;
            IsRefreshing = isRefreshing;
        }

        private async Task LoadDataFromYesterdayUntilNow()
            => await LoadData(DateRange.FromYesterdayUntilNow());
    }
}