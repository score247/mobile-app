using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeaguesOfCountryViewModel : ViewModelBase
    {
        private readonly Func<string, string> buildFlagFunction;
        private readonly ILeagueService leagueService;

        public LeaguesOfCountryViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            leagueService = DependencyResolver.Resolve<ILeagueService>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
            IsBusy = false;
        }

        public IEnumerable<LeagueViewModel> LeaguesItems { get; private set; }

        public string CountryCode { get; protected set; }

        public string LeagueName { get; private set; }

        public string CountryFlag { get; private set; }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; protected set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?["Country"] is LeagueCategory country)
            {
                LeagueName = country.CountryName.ToUpperInvariant();
                CountryCode = country.CountryCode;
                CountryFlag = buildFlagFunction(CountryCode);
            }

            if (parameters?["LeaguesOfCountry"] is IEnumerable<ILeague> leagues)
            {
                BuildLeagueItems(leagues);
            }
        }

        protected virtual async Task OnRefreshAsync()
        {
            await LoadDataAsync(UpdateCountryLeaguesAsync, false);

            IsRefreshing = false;
        }

        protected virtual async Task UpdateCountryLeaguesAsync()
        {
            var leagues = (await leagueService.GetCountryLeagues(CountryCode, CurrentLanguage))?
                .Where(league => !string.IsNullOrEmpty(league.SeasonId));
            BuildLeagueItems(leagues);

            IsRefreshing = false;
            HasData = true;
        }

        private void BuildLeagueItems(IEnumerable<ILeague> leagues)
        {
            LeaguesItems = leagues
                .OrderBy(league => league.Order)
                .Select(league => new LeagueViewModel(
                    NavigationService,
                    DependencyResolver,
                    buildFlagFunction,
                    league,
                    league.Name,
                    league.CountryCode,
                    false));
        }
    }
}