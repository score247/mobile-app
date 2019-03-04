namespace League.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using League.Models;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;

    public class LeagueDetailPageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private readonly ILeagueService leagueService;
        private bool isRefreshingMatchList;
        private string currentLeagueId;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public bool IsRefreshingMatchList
        {
            get => isRefreshingMatchList;
            set => SetProperty(ref isRefreshingMatchList, value);
        }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                leagueService.GetMatches(currentLeagueId);
                IsRefreshingMatchList = false;
            });

        public LeagueDetailPageViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            this.leagueService = leagueService;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            currentLeagueId = parameters["id"] as string;
            var matches = leagueService.GetMatches(currentLeagueId);

            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}