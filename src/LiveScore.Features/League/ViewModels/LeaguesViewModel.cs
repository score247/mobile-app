using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.ViewModels.LeagueItemViewModels;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeaguesViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly Func<string, string> buildFlagFunction;
        private bool firstLoad = true;

        public LeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            leagueService = DependencyResolver.Resolve<ILeagueService>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            LeagueGroups = new List<IGrouping<string, LeagueViewModel>>();
            RefreshCommand = new DelegateAsyncCommand(OnRefreshing);
        }

        public IList<IGrouping<string, LeagueViewModel>> LeagueGroups { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public override async Task OnNetworkReconnectedAsync()
        {
            await base.OnNetworkReconnectedAsync();
            await LoadDataAsync(LoadLeagues);
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();
            await LoadDataAsync(LoadLeagues);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive || !firstLoad)
            {
                return;
            }

            await LoadDataAsync(LoadLeagues);
            firstLoad = false;
        }

        private async Task OnRefreshing()
        {
            await LoadDataAsync(LoadLeagues);

            IsRefreshing = false;
        }

        public async Task LoadLeagues()
        {
            var leagues = (await leagueService.GetMajorLeaguesAsync(CurrentLanguage))?.ToList();

            if (leagues?.Any() == true)
            {
                BuildLeagueGroups(leagues);
            }

            HasData = false;
        }

        private void BuildLeagueGroups(IList<ILeague> leagues)
        {
            var topLeagues = new List<ILeague>(leagues.OrderBy(league => league.Order).Take(10));
            var topLeaguesGroup = BuildTopLeaguesGroup(topLeagues);
            var allLeaguesGroup = BuildAllLeaguesGroup(leagues);
            var leagueGroups = topLeaguesGroup.Concat(allLeaguesGroup);

            LeagueGroups = new List<IGrouping<string, LeagueViewModel>>(leagueGroups);
        }

        private IEnumerable<IGrouping<string, LeagueViewModel>> BuildTopLeaguesGroup(IEnumerable<ILeague> topLeagues)
            => topLeagues
                .Select(league => new LeagueViewModel(NavigationService, DependencyResolver, buildFlagFunction, league, league.CountryCode))
                .GroupBy(_ => AppResources.Popular);

        private IEnumerable<IGrouping<string, LeagueViewModel>> BuildAllLeaguesGroup(IEnumerable<ILeague> leagues)
        {
            var orderedLeagues = leagues
                .OrderBy(league => league.CountryName)
                .ThenBy(league => league.Name)
                .ToList();

            var internationalLeagues = orderedLeagues.Where(league => league.IsInternational);
            var internationalLeaguesGroup = BuildInternationalLeaguesGroup(internationalLeagues);

            var countryLeagues = orderedLeagues.Where(league => !league.IsInternational);
            var countryGroups = BuildCountryLeaguesGroup(countryLeagues);

            var allLeagues = internationalLeaguesGroup.Concat(countryGroups);

            return allLeagues.GroupBy(_ => AppResources.AllLeagues);
        }

        private IEnumerable<LeagueViewModel> BuildInternationalLeaguesGroup(IEnumerable<ILeague> internationalLeagues)
        {
            var internationalCategory = new LeagueCategory
            {
                Name = AppResources.International
            };

            return new List<CountryViewModel>
            {
                new CountryViewModel(
                    NavigationService,
                    DependencyResolver,
                    internationalCategory,
                    internationalLeagues,
                    buildFlagFunction)
            };
        }

        private IEnumerable<LeagueViewModel> BuildCountryLeaguesGroup(IEnumerable<ILeague> countryLeagues)
        {
            var countriesGroup = countryLeagues
                .GroupBy(league => new LeagueCategory
                {
                    CountryCode = league.CountryCode,
                    Name = league.CountryName
                })
                .ToDictionary(league => league.Key);

            return countriesGroup.Keys.Select(country => new CountryViewModel(
                           NavigationService,
                           DependencyResolver,
                           country,
                           countriesGroup[country],
                           buildFlagFunction));
        }
    }
}