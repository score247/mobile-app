namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Models;
    using Common.Services;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Views;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<dynamic, Match>> groupMatches;
        private ObservableCollection<DateTime> calendarItems;
        private bool isRefreshingMatchList;
        private readonly IMatchService matchService;

        public ScorePageViewModel(
            INavigationService navigationService,
            IMatchService matchService)
        : base(navigationService)
        {
            this.matchService = matchService;
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

        public ObservableCollection<DateTime> CalendarItems
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
                GetMatches();
                IsRefreshingMatchList = false;
            });

        public override void OnAppearing()
        {
            GetMatches();
            GenerateCalendar();
        }

        private void GetMatches()
        {
            var matches = matchService.GetDailyMatches(DateTime.Today.AddDays(-1)).Result;

            GroupMatches = new ObservableCollection<IGrouping<dynamic, Match>>(
                matches.GroupBy(m => new { m.Event.League.Name, m.Event.EventDate.Day }));
        }

        private void GenerateCalendar()
        {
            CalendarItems = new ObservableCollection<DateTime>
            {
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1),
                DateTime.Today,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(2),
                DateTime.Today.AddDays(3)
            };
        }
    }
}