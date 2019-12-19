using LiveScore.Core.Models.Leagues;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels.Leagues
{
    public class LeagueItemViewModel : ViewModelBase
    {
        public LeagueItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            ILeague league)
            : base(navigationService, dependencyResolver)
        {
            League = league;
        }

        public ILeague League { get; }
    }
}