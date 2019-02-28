namespace Score.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Settings;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private ObservableCollection<DateTime> calendarItems;
        private bool isRefreshingMatchList;
        private readonly IScoreService scoreService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
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

        public ScorePageViewModel(INavigationService navigationService, IScoreService scoreService)
            : base(navigationService)
        {
            this.scoreService = scoreService;
        }

        public override void OnAppearing()
        {
            GetMatches();
            GenerateCalendar();
        }

        private void GetMatches()
        {
            var matches = scoreService.GetAll(Settings.CurrentSportId).ToList();
            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
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