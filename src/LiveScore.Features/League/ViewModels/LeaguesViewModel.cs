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
using Prism.Commands;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeaguesViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly Func<string, string> buildFlagFunction;
        private bool firstLoad = true;
        private IList<ILeague> currentLeagues;
        private IList<IGrouping<LeagueCategory, LeagueViewModel>> currentLeagueGroups;

        public LeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            leagueService = DependencyResolver.Resolve<ILeagueService>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            LeagueGroups = new List<IGrouping<LeagueCategory, LeagueViewModel>>();
            RefreshCommand = new DelegateAsyncCommand(OnRefreshing);
            SearchCommand = new DelegateCommand<string>(OnSearch);
        }

        public IList<IGrouping<LeagueCategory, LeagueViewModel>> LeagueGroups { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public DelegateCommand<string> SearchCommand { get; }

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
            await LoadDataAsync(LoadLeagues, false);

            IsRefreshing = false;
        }

        public async Task LoadLeagues()
        {
            currentLeagues = (await leagueService.GetMajorLeaguesAsync(CurrentLanguage))?.ToList();

            if (currentLeagues?.Any() == true)
            {
                BuildLeagueGroups(currentLeagues);
            }

            HasData = false;
        }

        private void BuildLeagueGroups(IList<ILeague> leagues)
        {
            var topLeagues = new List<ILeague>(leagues
                    .OrderBy(league => league.Order)
                    .Where(league => league.SeasonDates.EndDate.AddDays(7) >= DateTime.Today)
                    .Take(6));

            var topLeaguesGroup = BuildTopLeaguesGroup(topLeagues);
            var allLeaguesGroup = BuildAllLeaguesGroup(leagues);
            var leagueGroups = topLeaguesGroup.Concat(allLeaguesGroup);

            LeagueGroups = new List<IGrouping<LeagueCategory, LeagueViewModel>>(leagueGroups);
            currentLeagueGroups = new List<IGrouping<LeagueCategory, LeagueViewModel>>(LeagueGroups);
        }

        private IEnumerable<IGrouping<LeagueCategory, LeagueViewModel>> BuildTopLeaguesGroup(IEnumerable<ILeague> topLeagues)
            => topLeagues
                .Select(league => new LeagueViewModel(NavigationService, DependencyResolver, buildFlagFunction, league, league.CountryCode))
                .GroupBy(_ => new LeagueCategory(string.Empty, AppResources.Popular, AppResources.Popular));

        private IEnumerable<IGrouping<LeagueCategory, LeagueViewModel>> BuildAllLeaguesGroup(IEnumerable<ILeague> leagues)
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

            return allLeagues.GroupBy(_ => new LeagueCategory(string.Empty, AppResources.AllLeagues, AppResources.AllLeagues));
        }

        private IEnumerable<LeagueViewModel> BuildInternationalLeaguesGroup(IEnumerable<ILeague> internationalLeagues)
        {
            var internationalCategory =
                new LeagueCategory(string.Empty, AppResources.International, AppResources.International);

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
                .GroupBy(league => new LeagueCategory(string.Empty, league.CountryName, league.CountryCode));

            return countriesGroup.Select(country => new CountryViewModel(
                           NavigationService,
                           DependencyResolver,
                           country.Key,
                           country.ToList(),
                           buildFlagFunction));
        }

        private void OnSearch(string searchText)
        {
            if (!string.IsNullOrEmpty((searchText)))
            {
                var lowerCaseSearchText = searchText.ToLowerInvariant();
                LeagueGroups.Clear();

                var filteredLeagues = currentLeagues.Where(league =>
                        league.Name.ToLowerInvariant().Contains(lowerCaseSearchText)
                        || league.CountryName.ToLowerInvariant().Contains(lowerCaseSearchText));

                var filterLeagueViewModels = filteredLeagues.Select(league =>
                    new LeagueViewModel(NavigationService, DependencyResolver, buildFlagFunction, league,
                        league.CountryCode));

                var filteredLeagueViewModelsGroups = filterLeagueViewModels
                    .GroupBy(
                        viewModel => new LeagueCategory(string.Empty, viewModel.CountryName, viewModel.CountryName));

                LeagueGroups = new List<IGrouping<LeagueCategory, LeagueViewModel>>(filteredLeagueViewModelsGroups);
            }
            else
            {
                LeagueGroups = new List<IGrouping<LeagueCategory, LeagueViewModel>>(currentLeagueGroups);
            }
        }
    }
}