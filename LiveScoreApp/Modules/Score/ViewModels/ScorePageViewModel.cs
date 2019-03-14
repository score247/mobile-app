namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Settings;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<dynamic, Match>> groupMatches;
        private ObservableCollection<CalendarDate> calendarItems;
        private bool isRefreshingMatchList;
        private bool isLoadingMatches;
        private readonly IMatchService matchService;
        private CalendarDate selectedCalendarDate;

        public ScorePageViewModel(
            INavigationService navigationService,
            IMatchService matchService)
        : base(navigationService)
        {
            this.matchService = matchService;

            Task.Run(Initialize).Wait();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport)
            {
                await Initialize();
            }
        }

        #region BINDING PROPERTIES

        public CalendarDate SelectedCalendarDate
        {
            get { return selectedCalendarDate; }
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

        #endregion

        #region BINDING COMMAND

        public DelegateCommand SelectMatchCommand
           => new DelegateCommand(async () => await NavigationService.NavigateAsync(nameof(MatchInfoPage)));

        public DelegateCommand RefreshCommand
            => new DelegateCommand(async () =>
            {
                IsRefreshingMatchList = true;
                await LoadMatches(SelectedCalendarDate.Date, showLoadingIndicator: false);
                IsRefreshingMatchList = false;
            });

        public DelegateCommand SelectDateCommand => new DelegateCommand(OnSelectDateCommandExecuted);

        private async void OnSelectDateCommandExecuted()
        {
            LoadCalendar(SelectedCalendarDate.Date);
            await LoadMatches(SelectedCalendarDate.Date);
        }

        #endregion

        private async Task Initialize()
        {
            LoadCalendar(DateTime.Now);
            await LoadMatches(DateTime.Now);
        }

        private async Task LoadMatches(DateTime currentDate, bool showLoadingIndicator = true)
        {
            IsLoadingMatches = showLoadingIndicator;
            var matches = await matchService.GetDailyMatches(currentDate);
            IsLoadingMatches = !IsLoadingMatches && showLoadingIndicator;

            GroupMatches = new ObservableCollection<IGrouping<dynamic, Match>>(
                matches.GroupBy(m => new { m.Event.League.Name, m.Event.ShortEventDate }));

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