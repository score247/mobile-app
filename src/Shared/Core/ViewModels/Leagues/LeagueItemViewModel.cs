using System;
using LiveScore.Core.Models.Leagues;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels.Leagues
{
    public class LeagueItemViewModel : ViewModelBase
    {
        public LeagueItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            ILeague league,
            Func<string, string> buildFlagFunction)
            : base(navigationService, dependencyResolver)
        {
            League = league;
            LeagueFlag = buildFlagFunction(league.CountryCode);
        }

        public ILeague League { get; }

        public string LeagueFlag { get; }
    }
}