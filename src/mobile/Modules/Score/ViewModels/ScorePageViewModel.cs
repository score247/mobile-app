namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;

    public class ScorePageViewModel : ViewModelBase
    {
        private int OldDateCalendarItemCount = 3;
        private int NewDateCalendarItemCount = 7;
        private ObservableCollection<IGrouping<dynamic, Match>> groupMatches;
        private ObservableCollection<CalendarDate> calendarItems;
        private bool isRefreshingMatchList;
        private bool isLoadingMatches;
        private bool isRefreshingCalendarList;
        private readonly IMatchService matchService;
        private CalendarDate selectedCalendarDate;

        public ScorePageViewModel(
            INavigationService navigationService,
            IMatchService matchService)
        : base(navigationService)
        {
            this.matchService = matchService;
            InitializeCommands();
            SelectHome = true;
            Task.Run(InitializeData).Wait();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport)
            {
                await InitializeData();
            }
        }

        public CalendarDate SelectedCalendarDate
        {
            get { return selectedCalendarDate ?? new CalendarDate { Date = DateTime.MinValue, IsSelected = true }; }
            set { SetProperty(ref selectedCalendarDate, value); }
        }

        public ObservableCollection<IGrouping<dynamic, Match>> GroupMatches
        {
            get => groupMatches;
            set => SetProperty(ref groupMatches, value);
        }

        public bool IsLoadingMatches
        {
            get { return isLoadingMatches; }
            set { SetProperty(ref isLoadingMatches, value); }
        }

        public bool IsRefreshingMatchList
        {
            get => isRefreshingMatchList;
            set => SetProperty(ref isRefreshingMatchList, value);
        }

        public ObservableCollection<CalendarDate> CalendarItems
        {
            get => calendarItems;
            set => SetProperty(ref calendarItems, value);
        }

        public bool IsRefreshingCalendarList
        {
            get { return isRefreshingCalendarList; }
            set { SetProperty(ref isRefreshingCalendarList, value); }
        }

        private bool selectHome;

        public bool SelectHome
        {
            get { return selectHome; }
            set { SetProperty(ref selectHome, value); }
        }




        public DelegateCommand SelectMatchCommand { get; private set; }

        public DelegateCommand RefreshMatchListCommand { get; private set; }

        public DelegateCommand RefreshCalendarListCommand { get; private set; }

        public DelegateCommand LoadMoreCalendarCommand { get; private set; }

        public DelegateCommand SelectDateCommand { get; private set; }

        public DelegateCommand SelectHomeCommand { get; private set; }

        private void InitializeCommands()
        {
            SelectMatchCommand = new DelegateCommand(async ()
                => await NavigationService.NavigateAsync(nameof(MatchInfoPage)));

            RefreshMatchListCommand = new DelegateCommand(OnRefreshMatchListCommand);
            RefreshCalendarListCommand = new DelegateCommand(OnRefreshCalendarListCommand);
            LoadMoreCalendarCommand = new DelegateCommand(OnLoadMoreCalendarCommand);
            SelectDateCommand = new DelegateCommand(OnSelectDateCommand);
            SelectHomeCommand = new DelegateCommand(OnSelectHomeCommandExecuted);
        }

        private async void OnRefreshMatchListCommand()
        {
            await LoadMatches(SelectedCalendarDate.Date, showLoadingIndicator: false);
            IsRefreshingMatchList = false;
        }

        private void OnRefreshCalendarListCommand()
        {
            LoadCalendar(SelectedCalendarDate.Date, moreOldDay: 7);
            IsRefreshingCalendarList = false;
        }

        private void OnLoadMoreCalendarCommand()
        {
            if (NewDateCalendarItemCount <= 30)
            {
                LoadCalendar(SelectedCalendarDate.Date, moreNewDay: 3);
            }
        }

        private async void OnSelectDateCommand()
        {
            SelectHome = false;
            LoadCalendar(SelectedCalendarDate.Date);
            await LoadMatches(SelectedCalendarDate.Date);
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

        private async Task LoadMatches(DateTime currentDate, bool showLoadingIndicator = true)
        {
            IsLoadingMatches = showLoadingIndicator;

            var matches = await matchService.GetDailyMatches(currentDate);
            GroupMatches = new ObservableCollection<IGrouping<dynamic, Match>>(
                matches.GroupBy(m => new { m.Event.League.Name, m.Event.ShortEventDate }));

            IsLoadingMatches = !IsLoadingMatches && showLoadingIndicator;
        }

        private void LoadCalendar(DateTime currentDate, int moreOldDay = 0, int moreNewDay = 0)
        {
            var calendar = new ObservableCollection<CalendarDate>();

            NewDateCalendarItemCount += moreNewDay;
            OldDateCalendarItemCount += moreOldDay;

            for (int i = OldDateCalendarItemCount * -1; i <= NewDateCalendarItemCount; i++)
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