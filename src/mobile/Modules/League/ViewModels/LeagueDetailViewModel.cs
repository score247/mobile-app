namespace League.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.ViewModels;
    using Common.Models.MatchInfo;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using Common.Models;
    using System.Threading.Tasks;

    public class LeagueDetailViewModel : ViewModelBase
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

        public LeagueDetailViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            this.leagueService = leagueService;
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            currentLeagueId = parameters["id"] as string;

            var matches = await leagueService.GetMatches(currentLeagueId);

            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.Event.LeagueRound.Number));
        }
    }
}