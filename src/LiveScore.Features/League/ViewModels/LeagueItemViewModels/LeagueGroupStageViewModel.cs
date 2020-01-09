using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.NavigationParams;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class LeagueGroupStageViewModel : LeagueViewModel
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public LeagueGroupStageViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            Func<string, string> buildFlagFunction,
            LeagueDetailNavigationParameter leagueDetail,
            string countryFlag,
            string leagueRoundGroup,
            string groupStageName,
            bool hasStanding)
            : base(navigationService, dependencyResolver, buildFlagFunction, null, null, false)
        {
            LeagueDetail = leagueDetail;
            CountryFlag = countryFlag;
            LeagueName = groupStageName.ToUpperInvariant();
            LeagueRoundGroup = leagueRoundGroup;
            HasStanding = hasStanding;
            LeagueTapped = new DelegateAsyncCommand(OnTapGroupAsync);
        }

#pragma warning restore S107 // Methods should not have too many parameters

        public LeagueDetailNavigationParameter LeagueDetail { get; }

        public string LeagueRoundGroup { get; }

        public bool HasStanding { get; }

        private async Task OnTapGroupAsync()
        {
            var parameters = new NavigationParameters
                {
                    { "League", GetLeagueDetailNavigationParameter() },
                    { "CountryFlag", CountryFlag }
                };

            var navigateResult = await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters);

            if (!navigateResult.Success)
            {
                await LoggingService.LogExceptionAsync(navigateResult.Exception);
            }
        }

        private LeagueDetailNavigationParameter GetLeagueDetailNavigationParameter()
            => new LeagueDetailNavigationParameter(
                   LeagueDetail.Id,
                   LeagueName,
                   LeagueDetail.Order,
                   LeagueDetail.CountryCode,
                   LeagueDetail.IsInternational,
                   LeagueRoundGroup,
                   LeagueDetail.SeasonId,
                   HasStanding);
    }
}