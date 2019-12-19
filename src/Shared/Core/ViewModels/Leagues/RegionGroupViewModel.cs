using System;
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
            IEnumerable<ILeague> leagues,
            Func<string, string> buildFlagFunction)
            : base(navigationService, dependencyResolver)
        {
            Region = leagueCategory;
            RegionLeagues = leagues;
            RegionFlag = buildFlagFunction(Region.CountryCode);
        }

        public IEnumerable<ILeague> RegionLeagues { get; }

        public LeagueCategory Region { get; }
        public string RegionFlag { get; }
    }
}