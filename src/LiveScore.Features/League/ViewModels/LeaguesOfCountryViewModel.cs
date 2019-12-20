using System;
using System.Collections.Generic;
using System.Linq;
using LiveScore.Common;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeaguesOfCountryViewModel : ViewModelBase
    {
        private readonly Func<string, string> buildFlagFunction;

        public LeaguesOfCountryViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
        }

        public IEnumerable<LeagueViewModel> LeaguesItems { get; private set; }

        public string CountryCode { get; protected set; }

        public string LeagueName { get; private set; }

        public string LeagueFlag { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Country"] is LeagueCategory country)
            {
                LeagueName = country.Name.ToUpperInvariant();
                CountryCode = country.CountryCode;
                LeagueFlag = buildFlagFunction(CountryCode);
            }

            if (parameters?["LeaguesOfCountry"] is IEnumerable<ILeague> leagues)
            {
                LeaguesItems = leagues
                    .OrderBy(league => league.Name)
                    .Select(league => new LeagueViewModel(NavigationService, DependencyResolver, buildFlagFunction, league, league.CountryCode, false));
            }
        }
    }
}