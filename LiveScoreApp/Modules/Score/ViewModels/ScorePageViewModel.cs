namespace Score.ViewModels
{
    using LiveScoreApp.Core.Settings;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private bool isRefreshingMatchList;
        private readonly IScoreService scoreService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public bool IsRefreshingMatchList
        {
            get { return isRefreshingMatchList; }
            set { SetProperty(ref isRefreshingMatchList, value); }
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
        }

        private void GetMatches()
        {
            var matches = scoreService.GetAll(Settings.CurrentSportId).ToList();
            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}