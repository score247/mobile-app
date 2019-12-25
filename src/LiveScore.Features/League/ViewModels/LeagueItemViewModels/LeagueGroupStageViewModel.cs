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
            string leagueFlag,
            string leagueRoundGroup,
            string groupStageName)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(navigationService, dependencyResolver, buildFlagFunction, null, null, null, false)
        {
            LeagueDetail = leagueDetail;
            LeagueFlag = leagueFlag;
            LeagueName = groupStageName;
            LeagueRoundGroup = leagueRoundGroup;
            LeagueTapped = new DelegateAsyncCommand(OnTapGroupAsync);
        }

        public LeagueDetailNavigationParameter LeagueDetail { get; private set; }

        public string LeagueRoundGroup { get; }

        private async Task OnTapGroupAsync()
        {
            var parameters = new NavigationParameters
                {
                    { "League", GetLeagueDetailNavigationParameter() },
                    { "LeagueSeasonId", LeagueSeasonId },
                    { "LeagueRoundGroup", LeagueRoundGroup },
                    { "LeagueGroupName", LeagueName },
                    { "CountryFlag", LeagueFlag }
                };

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }

        private LeagueDetailNavigationParameter GetLeagueDetailNavigationParameter()
            => new LeagueDetailNavigationParameter(
                   LeagueDetail.Id,
                   LeagueName,
                   LeagueDetail.Order,
                   LeagueDetail.CountryCode,
                   LeagueDetail.IsInternational,
                   LeagueRoundGroup,
                   LeagueDetail.SeasonId);
    }
}