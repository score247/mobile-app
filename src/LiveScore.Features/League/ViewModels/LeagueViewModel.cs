using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.ViewModels.Leagues;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels
{
    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly Func<string, string> buildFlagFunction;

        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            ILeagueService leagueService)
                : base(navigationService, serviceLocator)
        {
            this.leagueService = leagueService;
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            LeagueGroups = new ObservableCollection<IGrouping<string, ViewModelBase>>();
        }

        public ObservableCollection<IGrouping<string, ViewModelBase>> LeagueGroups { get; set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!IsActive)
            {
                return;
            }

            await BuildLeagueGroup();
        }

        public async Task BuildLeagueGroup()
        {
            var leagues = (await leagueService.GetMajorLeaguesAsync(CurrentLanguage))?.ToList();

            if (leagues != null)
            {
                BuildLeagueGroups(leagues);
            }

            IsBusy = false;
            HasData = IsNotBusy;
        }

        private void BuildLeagueGroups(List<ILeague> leagues)
        {
            var topLeagues = new ObservableCollection<ILeague>(leagues.OrderBy(league => league.Order).Take(10));

            var topLeagueGroup = BuildTopLeagueGroup(topLeagues);

            var countryLeagueGroup = BuildAllLeagueGroup(leagues);

            var leagueGroups = topLeagueGroup.Concat(countryLeagueGroup);

            LeagueGroups = new ObservableCollection<IGrouping<string, ViewModelBase>>(leagueGroups);
        }

        private IEnumerable<IGrouping<string, ViewModelBase>> BuildTopLeagueGroup(ObservableCollection<ILeague> topLeagues)
        {
            return topLeagues
                .Select(league => new LeagueItemViewModel(NavigationService, DependencyResolver, buildFlagFunction, league, true))
                .GroupBy(_ => AppResources.Popular);
        }

        private IEnumerable<IGrouping<string, ViewModelBase>> BuildAllLeagueGroup(IEnumerable<ILeague> leagues)
        {
            var orderedLeague = leagues
                .OrderBy(league => league.CountryName)
                .ThenBy(league => league.Name);

            var internationalLeagues = orderedLeague.Where(league => league.IsInternational);

            var internationalLeaguesGroup = BuildInternationalLeaguesGroup(internationalLeagues);

            var countryLeagues = orderedLeague.Where(league => !league.IsInternational);
            var countryGroups = BuildCountryLeaguesGroup(countryLeagues);

            var allLeagues = internationalLeaguesGroup.Concat(countryGroups);

            return allLeagues.GroupBy(_ => AppResources.AllLeagues);
        }

        private IEnumerable<ViewModelBase> BuildInternationalLeaguesGroup(IEnumerable<ILeague> internationalLeagues)
        {
            var internationalCategory = new LeagueCategory
            {
                Name = AppResources.International
            };

            var internationalViewModels = new List<RegionGroupViewModel>
            {
                new RegionGroupViewModel(
                    NavigationService,
                    DependencyResolver,
                    internationalCategory,
                    internationalLeagues,
                    buildFlagFunction)
            };

            return internationalViewModels;
        }

        private IEnumerable<ViewModelBase> BuildCountryLeaguesGroup(IEnumerable<ILeague> countryLeagues)
        {
            var countryGroup = countryLeagues
                .GroupBy(league => new LeagueCategory
                {
                    CountryCode = league.CountryCode,
                    Name = league.CountryName
                })
                .ToDictionary(league
                => league.Key);
            var countryLeaguesItems = new List<ViewModelBase>();
            foreach (var country in countryGroup.Keys)
            {
                countryLeaguesItems.Add(
                    new RegionGroupViewModel(
                        NavigationService,
                        DependencyResolver,
                        country,
                        countryGroup[country],
                        buildFlagFunction)
                    );
            }

            return countryLeaguesItems;
        }
    }
}