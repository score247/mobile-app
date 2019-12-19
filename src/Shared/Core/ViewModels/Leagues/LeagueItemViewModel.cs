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
            Func<string, string> buildFlagFunction,
            ILeague league,
            bool isShowFlag)
            : base(navigationService, dependencyResolver)
        {
            League = league;
            LeagueFlag = buildFlagFunction(league.CountryCode);
            IsShowFlag = isShowFlag;
        }

        public ILeague League { get; }

        public bool IsShowFlag { get; }

        public string LeagueFlag { get; }
    }
}