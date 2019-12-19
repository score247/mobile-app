using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels.Leagues
{
    public class RegionGroupViewModel : ViewModelBase
    {
        public RegionGroupViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            LeagueCategory leagueCategory,
            IEnumerable<ILeague> leagues)
            : base(navigationService, dependencyResolver)
        {
            Region = leagueCategory;
            RegionLeagues = leagues;
        }

        public IEnumerable<ILeague> RegionLeagues { get; }

        public LeagueCategory Region { get; }
    }
}