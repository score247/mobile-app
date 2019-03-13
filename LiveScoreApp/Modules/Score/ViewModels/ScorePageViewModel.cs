namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        private readonly IMatchService matchService;
        private CalendarDate selectedCalendarDate;

        public ScorePageViewModel(
            INavigationService navigationService,
            IMatchService matchService)
        : base(navigationService)
        {
            this.matchService = matchService;
        }

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

        public DelegateCommand SelectMatchCommand
            => new DelegateCommand(async () => await NavigationService.NavigateAsync(nameof(MatchInfoPage)));

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                GetMatches(SelectedCalendarDate.Date);
                IsRefreshingMatchList = false;
            });

        public DelegateCommand SelectDateCommand => new DelegateCommand(OnSelectDateCommandExecuted);

        private void OnSelectDateCommandExecuted()
        {
            GenerateCalendar(SelectedCalendarDate.Date);
            GetMatches(SelectedCalendarDate.Date);
        }

        public override void OnAppearing()
        {
            GetMatches(DateTime.Now);
            GenerateCalendar(DateTime.Now);
        }

        private void GetMatches(DateTime currentDate)
        {
            var matches = matchService.GetDailyMatches(currentDate).Result;

            GroupMatches = new ObservableCollection<IGrouping<dynamic, Match>>(
                matches.GroupBy(m => new { m.Event.League.Name, m.Event.ShortEventDate }));
        }

        private void GenerateCalendar(DateTime currentDate)
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