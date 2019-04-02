﻿namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;

    public class ScoresViewModel : ViewModelBase
    {
        private const int MaximumCalendarItemCount = 30;
        private const int MoreOldDayCount = 7;
        private const int MoreNewDayCount = 3;
        private readonly IMatchService matchService;
        private int oldDateCalendarItemCount = 3;
        private int newDateCalendarItemCount = 7;
        private ObservableCollection<IGrouping<dynamic, Match>> groupMatches;
        private ObservableCollection<CalendarDate> calendarItems;
        private bool isRefreshingMatchList;
        private bool isLoadingMatches;
        private bool isRefreshingCalendarList;
        private CalendarDate selectedCalendarDate;
        private bool selectHome;

        public ScoresViewModel(
            INavigationService navigationService,
            IMatchService matchService)
        : base(navigationService)
        {
            this.matchService = matchService;
            InitializeCommands();
            SelectHome = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var changeSport = parameters.GetValue<bool>("changeSport");

            if (changeSport || GroupMatches == null)
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

        public bool SelectHome
        {
            get { return selectHome; }
            set { SetProperty(ref selectHome, value); }
        }

        public DelegateAsyncCommand<Match> SelectMatchCommand { get; private set; }

        public DelegateAsyncCommand RefreshMatchListCommand { get; private set; }

        public DelegateCommand RefreshCalendarListCommand { get; private set; }

        public DelegateCommand LoadMoreCalendarCommand { get; private set; }

        public DelegateAsyncCommand SelectDateCommand { get; private set; }

        public DelegateCommand SelectHomeCommand { get; private set; }

        private void InitializeCommands()
        {
            SelectMatchCommand = new DelegateAsyncCommand<Match>(async (item) =>
            {
                var navigationParams = new NavigationParameters
                {
                    { nameof(Match), item }
                };

                await NavigationService.NavigateAsync(nameof(MatchDetailView), navigationParams);
            });

            RefreshMatchListCommand = new DelegateAsyncCommand(OnRefreshMatchListCommandAsync);
            RefreshCalendarListCommand = new DelegateCommand(OnRefreshCalendarListCommand);
            LoadMoreCalendarCommand = new DelegateCommand(OnLoadMoreCalendarCommand);
            SelectDateCommand = new DelegateAsyncCommand(OnSelectDateCommandAsync);
            SelectHomeCommand = new DelegateCommand(OnSelectHomeCommandExecuted);
        }

        private async Task OnRefreshMatchListCommandAsync()
        {
            var date = SelectedCalendarDate.Date;

            if (date.Date == DateTime.MinValue)
            {
                date = DateTime.Today;
            }

            await LoadMatches(date, showLoadingIndicator: false, forceFetchNewData: true).ConfigureAwait(false);
            IsRefreshingMatchList = false;
        }

        private void OnRefreshCalendarListCommand()
        {
            LoadCalendar(SelectedCalendarDate.Date, moreOldDay: MoreOldDayCount);
            IsRefreshingCalendarList = false;
        }

        private void OnLoadMoreCalendarCommand()
        {
            if (newDateCalendarItemCount <= MaximumCalendarItemCount)
            {
                LoadCalendar(SelectedCalendarDate.Date, moreNewDay: MoreNewDayCount);
            }
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

            var matches = await matchService.GetDailyMatches(currentDate, forceFetchNewData);

            GroupMatches = new ObservableCollection<IGrouping<dynamic, Match>>(
                      matches.GroupBy(m => new { m.Event.League.Name, m.Event.ShortEventDate }));

            IsLoadingMatches = !IsLoadingMatches && showLoadingIndicator;
        }

        private void LoadCalendar(DateTime currentDate, int moreOldDay = 0, int moreNewDay = 0)
        {
            var calendar = new ObservableCollection<CalendarDate>();

            newDateCalendarItemCount += moreNewDay;
            oldDateCalendarItemCount += moreOldDay;

            for (int i = oldDateCalendarItemCount * -1; i <= newDateCalendarItemCount; i++)
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