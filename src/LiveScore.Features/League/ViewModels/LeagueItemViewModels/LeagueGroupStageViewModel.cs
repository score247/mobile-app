using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
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
            string leagueId,
            string leagueSeasonId,
            string leagueFlag,
            string leagueRoundGroup,
            string groupStageName)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(navigationService, dependencyResolver, buildFlagFunction, null, null, null, false)
        {
            LeagueId = leagueId;
            LeagueSeasonId = leagueSeasonId;
            LeagueFlag = leagueFlag;
            LeagueName = groupStageName;
            LeagueRoundGroup = leagueRoundGroup;
            LeagueTapped = new DelegateAsyncCommand(OnTapGroupAsync);
        }

        public string LeagueRoundGroup { get; }

        private async Task OnTapGroupAsync()
        {
            var parameters = new NavigationParameters
                {
                    { "LeagueId", LeagueId },
                    { "LeagueSeasonId", LeagueSeasonId },
                    { "LeagueRoundGroup", LeagueRoundGroup },
                    { "LeagueGroupName", LeagueName },
                    { "CountryFlag", LeagueFlag }
                };

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }
    }
}