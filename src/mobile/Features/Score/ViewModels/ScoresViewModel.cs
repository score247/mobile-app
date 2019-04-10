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
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Models;
    using LiveScore.Score.Views;
    using Prism.Commands;
    using Prism.Navigation;

    public class ScoresViewModel : ViewModelBase
    {
        private IMatchService matchService;

        public ScoresViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
        : base(navigationService, globalFactory, settingsService)
        {
            InitializeCommands();
            SelectHome = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || GroupMatches == null)
            {
                InitServicesBySportType();
                await InitializeData();
            }
        }

        public CalendarDate SelectedCalendarDate { get; set; }

        public ObservableCollection<IGrouping<dynamic, IMatch>> GroupMatches { get; set; }

        public bool IsLoadingMatches { get; set; }

        public bool IsRefreshingMatchList { get; set; }

        public ObservableCollection<CalendarDate> CalendarItems { get; set; }

        public bool SelectHome { get; set; }

        public DelegateAsyncCommand<IMatch> SelectMatchCommand { get; private set; }

        public DelegateAsyncCommand RefreshMatchListCommand { get; private set; }

        public DelegateAsyncCommand SelectDateCommand { get; private set; }

        public DelegateCommand SelectHomeCommand { get; private set; }

        private void InitializeCommands()
        {
            SelectMatchCommand = new DelegateAsyncCommand<IMatch>(async (item) =>
            {
                var navigationParams = new NavigationParameters
                {
                    { nameof(IMatch), item }
                };
                await NavigationService.NavigateAsync(nameof(MatchDetailView), navigationParams);
            });

            RefreshMatchListCommand = new DelegateAsyncCommand(OnRefreshMatchListCommandAsync);
            SelectDateCommand = new DelegateAsyncCommand(OnSelectDateCommandAsync);
            SelectHomeCommand = new DelegateCommand(OnSelectHomeCommandExecuted);
        }

        private void InitServicesBySportType()
        {
            matchService = GlobalFactoryProvider
                .SportServiceFactoryProvider
                .GetInstance((SportType)SettingsService.CurrentSportId)
                .CreateMatchService();
        }

        private async Task OnRefreshMatchListCommandAsync()
        {
            var date = SelectedCalendarDate == null ? DateTime.Today : SelectedCalendarDate.Date;
            await LoadMatches(date, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshingMatchList = false;
        }

        private async Task OnSelectDateCommandAsync()
        {
            SelectHome = false;
            LoadCalendar(SelectedCalendarDate.Date);
            await LoadMatches(SelectedCalendarDate.Date).ConfigureAwait(false);
        }

        private void OnSelectHomeCommandExecuted()
        {
            SelectHome = true;
            LoadCalendar(DateTime.MinValue);

            // TODO: Load matches for home
        }

        private async Task InitializeData()
        {
            LoadCalendar(DateTime.MinValue);
            await LoadMatches(DateTime.Now);
        }

        private async Task LoadMatches(DateTime currentDate, bool showLoadingIndicator = true, bool forceFetchNewData = false)
        {
            IsLoadingMatches = showLoadingIndicator;

            var matches = await matchService.GetDailyMatches(currentDate, currentDate, forceFetchNewData);

            GroupMatches = new ObservableCollection<IGrouping<dynamic, IMatch>>(
                      matches.GroupBy(m => new { m.League.Name, m.EventDate }));

            IsLoadingMatches = !IsLoadingMatches && showLoadingIndicator;
        }

        private void LoadCalendar(DateTime currentDate)
        {
            var calendar = new ObservableCollection<CalendarDate>();

            for (int i = -3; i <= 3; i++)
            {
                var date = DateTime.Today.AddDays(i);

                calendar.Add(
                    new CalendarDate
                    {
                        Date = date,
                        IsSelected = currentDate.Day == date.Day
                            && currentDate.Month == date.Month
                            && currentDate.Year == date.Year
                    });
            }

            CalendarItems = new ObservableCollection<CalendarDate>(calendar);
        }
    }
}