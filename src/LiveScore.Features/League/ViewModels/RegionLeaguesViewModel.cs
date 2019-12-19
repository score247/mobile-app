using System;
using System.Collections.Generic;
using System.Linq;
using LiveScore.Common;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.ViewModels;
using LiveScore.Core.ViewModels.Leagues;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class RegionLeaguesViewModel : ViewModelBase
    {
        private readonly Func<string, string> buildFlagUrlFunc;

        public RegionLeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver, true)
        {
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
        }

        public string RegionFlag { get; private set; }
        public IEnumerable<LeagueItemViewModel> LeaguesItems { get; private set; }
        public LeagueCategory Region { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Region"] is LeagueCategory region)
            {
                Region = region;
                RegionFlag = buildFlagUrlFunc(region.CountryCode);
            }

            if (parameters?["RegionLeagues"] is IEnumerable<ILeague> leagues)
            {
                LeaguesItems = leagues.Select(league => new LeagueItemViewModel(NavigationService, DependencyResolver, league, buildFlagUrlFunc));
            }
        }
    }
}