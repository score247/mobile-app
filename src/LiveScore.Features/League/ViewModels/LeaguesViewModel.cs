using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Extensions;
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
        private const string InternationalCode = "INTL";
        private readonly ILeagueService leagueService;
        private readonly Func<string, string> buildFlagFunction;
        private bool firstLoad = true;
        private IList<ILeague> currentLeagues;

        public LeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            leagueService = DependencyResolver.Resolve<ILeagueService>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            LeagueGroups = new List<IGrouping<LeagueGroupViewModel, LeagueViewModel>>();
            RefreshCommand = new DelegateAsyncCommand(OnRefreshing);
            SearchCommand = new DelegateCommand<string>(OnSearch);
        }

        public IList<IGrouping<LeagueGroupViewModel, LeagueViewModel>> LeagueGroups { get; private set; }

        public IList<IGrouping<LeagueGroupViewModel, LeagueViewModel>> SearchLeagueGroups { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public DelegateCommand<string> SearchCommand { get; }

        public bool ShowSearchLeagues { get; private set; }

        public bool ShowLeagues => !ShowSearchLeagues && IsNotBusy;

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
            currentLeagues = (await leagueService.GetMajorLeaguesAsync(CurrentLanguage))?
                .Where(league => !string.IsNullOrEmpty(league.SeasonId))
                .OrderBy(league => league.Order)
                .ToList();

            if (currentLeagues?.Any() == true)
            {
                BuildLeagueGroups(currentLeagues);
                HasData = true;
            }
            else
            {
                HasData = false;
            }
        }

        private void BuildLeagueGroups(IList<ILeague> leagues)
        {
            var topLeagues = new List<ILeague>(leagues
                    .Where(league => league.SeasonDates?.EndDate.AddDays(7) > DateTimeOffset.UtcNow)
                    .Take(8));

            var topLeaguesGroup = BuildTopLeaguesGroup(topLeagues);
            var allLeaguesGroup = BuildAllLeaguesGroup(leagues);
            var leagueGroups = topLeaguesGroup.Concat(allLeaguesGroup);

            LeagueGroups = new List<IGrouping<LeagueGroupViewModel, LeagueViewModel>>(leagueGroups);
        }

        private IEnumerable<IGrouping<LeagueGroupViewModel, LeagueViewModel>> BuildTopLeaguesGroup(IEnumerable<ILeague> topLeagues)
            => topLeagues
                .Select(league => new LeagueViewModel(NavigationService, DependencyResolver, buildFlagFunction, league, league.CombineCountryName(), league.CountryCode))
                .GroupBy(_ => new LeagueGroupViewModel(AppResources.Popular, AppResources.Popular));

        private IEnumerable<IGrouping<LeagueGroupViewModel, LeagueViewModel>> BuildAllLeaguesGroup(IList<ILeague> leagues)
        {
            var orderedLeagues = leagues
                .OrderBy(league => league.CountryName)
                .ToList();

            var internationalLeagues = orderedLeagues.Where(league => league.IsInternational);
            var internationalLeaguesGroup = BuildInternationalLeaguesGroup(internationalLeagues);

            var countryLeagues = orderedLeagues.Where(league => !league.IsInternational);
            var countryGroups = BuildCountryLeaguesGroup(countryLeagues);

            var allLeagues = internationalLeaguesGroup.Concat(countryGroups);

            return allLeagues.GroupBy(_ => new LeagueGroupViewModel(AppResources.AllLeagues, AppResources.AllLeagues));
        }

        private IEnumerable<LeagueViewModel> BuildInternationalLeaguesGroup(IEnumerable<ILeague> internationalLeagues)
        {
            var internationalCategory =
                new LeagueCategory(InternationalCode, AppResources.International);

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
                .GroupBy(league => new LeagueCategory(league.CountryCode, league.CountryName));

            return countriesGroup.Select(country => new CountryViewModel(
                           NavigationService,
                           DependencyResolver,
                           country.Key,
                           country.ToList(),
                           buildFlagFunction));
        }

        private void OnSearch(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                ShowSearchLeagues = true;
                var lowerCaseSearchText = searchText.ToLowerInvariant();

                if (currentLeagues == null)
                {
                    return;
                }

                var filteredLeagues = currentLeagues.Where(league =>
                    league.Name?.ToLowerInvariant().Contains(lowerCaseSearchText) == true
                    || league.CountryName?.ToLowerInvariant().Contains(lowerCaseSearchText) == true);

                var filterLeagueViewModels = filteredLeagues.Select(league =>
                    new LeagueViewModel(
                        NavigationService,
                        DependencyResolver,
                        buildFlagFunction,
                        league,
                        league.Name,
                        league.CountryCode));

                var filteredLeagueViewModelsGroups = filterLeagueViewModels
                    .GroupBy(viewModel => new LeagueGroupViewModel(viewModel.CountryCode, viewModel.CountryName, true, buildFlagFunction));

                SearchLeagueGroups = new List<IGrouping<LeagueGroupViewModel, LeagueViewModel>>(filteredLeagueViewModelsGroups);
            }
            else
            {
                ShowSearchLeagues = false;
            }
        }
    }
}