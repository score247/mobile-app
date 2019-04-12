namespace LiveScore.Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Constants;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Views;
    using Prism.Events;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private readonly DateRange currentDateRange;
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService,
            IEventAggregator eventAggregator) : base(navigationService, globalFactory, settingsService)
        {
            InitializeCommands();
            currentDateRange = InitDateRange();
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<DateBarHomeSelectedEvent>().Subscribe(OnSelectDateBarHome);
            EventAggregator.GetEvent<DateBarItemSelectedEvent>().Subscribe(OnSelectDateBarItem);
        }

        public bool IsLoading { get; set; }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> MatchGroups { get; set; }

        public DelegateAsyncCommand MatchRefreshCommand { get; private set; }

        public DelegateAsyncCommand<IMatch> MatchSelectCommand { get; private set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || MatchGroups == null)
            {
                matchService = GlobalFactoryProvider
                   .ServiceFactoryProvider
                   .GetInstance((SportType)SettingsService.CurrentSportId)
                   .CreateMatchService();

                await LoadMatches(currentDateRange);
            }
        }

        private void InitializeCommands()
        {
            MatchSelectCommand = new DelegateAsyncCommand<IMatch>(OnSelectMatchCommand);
            MatchRefreshCommand = new DelegateAsyncCommand(OnRefreshMatchCommandAsync);
        }

        private async Task OnSelectMatchCommand(IMatch match)
            => await NavigationService.NavigateAsync(nameof(MatchDetailView), new NavigationParameters
            {
                { nameof(IMatch), match }
            });

        private async Task OnRefreshMatchCommandAsync()
        {
            await LoadMatches(currentDateRange, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshing = false;
        }

        private async void OnSelectDateBarHome()
        {
            InitDateRange();
            await LoadMatches(currentDateRange);
        }

        private async void OnSelectDateBarItem(DateTime selectedDate)
        {
            currentDateRange.FromDate = selectedDate;
            currentDateRange.ToDate = selectedDate;
            await LoadMatches(currentDateRange);
        }

        private async Task LoadMatches(DateRange dateRangeValue, bool showLoadingIndicator = true, bool forceFetchNewData = false)
        {
            IsLoading = showLoadingIndicator;

            var dateRange = dateRangeValue ?? InitDateRange();
            var matches = await matchService.GetMatches(
                    SettingsService.CurrentSportId,
                    SettingsService.CurrentLanguage,
                    dateRange.FromDate,
                    dateRange.ToDate,
                    forceFetchNewData);
            MatchGroups = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matches.GroupBy(m => new { m.League.Name, m.EventDate.Day, m.EventDate.Month, m.EventDate.Year }));

            IsLoading = false;
        }

        private static DateRange InitDateRange()
            => new DateRange
            {
                FromDate = DateTime.Today.AddDays(-1),
                ToDate = DateTime.Today
            };
    }
}