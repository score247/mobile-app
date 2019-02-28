namespace League.ViewModels
{
    using Prism.Navigation;
    using System.Collections.ObjectModel;
    using System.Linq;
    using League.Models;
    using League.Services;

    public class LeagueDetailPageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private readonly ILeagueService leagueService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public LeagueDetailPageViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            this.leagueService = leagueService;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var tournamentId = parameters["id"] as string;
            var matches = leagueService.GetLeagueMatches(tournamentId);

            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}