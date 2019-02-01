namespace Tournament.ViewModels
{
    using Prism.Navigation;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Tournament.Models;
    using Tournament.Services;

    public class TournamentDetailPageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private ITournamentService tournamentService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public TournamentDetailPageViewModel(INavigationService navigationService, ITournamentService tournamentService)
            : base(navigationService)
        {
            this.tournamentService = tournamentService;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var tournamentId = parameters["id"] as string;
            var matches = tournamentService.GetTournamentMatches(tournamentId);

            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}