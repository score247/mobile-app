using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Leagues;
using Prism.Commands;
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
            RegionLeagueTapped = new DelegateAsyncCommand(OnTapRegionAsync);
        }

        private async Task OnTapRegionAsync()
        {
            var parameters = new NavigationParameters
            {
                { "Region", Region },
                { "RegionLeagues", RegionLeagues }
            };

            var navigated = await NavigationService
                .NavigateAsync("RegionLeaguesView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        public IEnumerable<ILeague> RegionLeagues { get; }

        public LeagueCategory Region { get; }
        public string RegionFlag { get; }
        public DelegateAsyncCommand RegionLeagueTapped { get; protected set; }
    }
}