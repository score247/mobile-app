using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.NavigationParams;
using MvvmHelpers;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels
{
    public class MatchGroupViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly int currentSportId;

        public MatchGroupViewModel(IMatch match, Func<string, string> buildFlagUrl, INavigationService navigationService, int currentSportId, bool enableTap = true)
        {
            if (match == null)
            {
                IsBusy = true;
                return;
            }

            LeagueId = match.LeagueId;
            LeagueGroupName = match.LeagueGroupName.ToUpperInvariant();
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
            CountryFlag = buildFlagUrl(match.CountryCode);
            CountryCode = match.CountryCode;
            LeagueOrder = match.LeagueOrder;
            Match = match;
            EnableTap = enableTap;

            this.navigationService = navigationService;
            this.currentSportId = currentSportId;
            TapLeagueCommand = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

        private IMatch Match { get; }

        public string LeagueId { get; }

        public string LeagueGroupName { get; }

        public string EventDate { get; }

        public string CountryCode { get; }

        public string CountryFlag { get; }

        public int LeagueOrder { get; }

        public bool EnableTap { get; }

        public DelegateAsyncCommand TapLeagueCommand { get; }

        private async Task OnTapLeagueAsync()
        {
            if (EnableTap)
            {
                var leagueNavitationParam = new LeagueDetailNavigationParameter(
                    LeagueId,
                    Match.LeagueGroupName,
                    Match.LeagueOrder,
                    Match.CountryCode,
                    Match.IsInternationalLeague,
                    Match.LeagueRoundGroup,
                    Match.LeagueSeasonId);

                var parameters = new NavigationParameters
                {
                    { "League", leagueNavitationParam },
                    { "CountryFlag", CountryFlag },
                    { "HomeId", Match.HomeTeamId },
                    { "AwayId", Match.HomeTeamId }
                };

                await navigationService
                    .NavigateAsync("LeagueDetailView" + currentSportId, parameters)
                    .ConfigureAwait(false);
            }
        }

        public override bool Equals(object obj)
            => (obj is MatchGroupViewModel actualObj) && LeagueGroupName == actualObj.LeagueGroupName && EventDate == actualObj.EventDate;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(LeagueGroupName))
            {
                if (string.IsNullOrWhiteSpace(LeagueId))
                {
                    if (string.IsNullOrWhiteSpace(CountryCode))
                    {
                        return Match?.GetHashCode() ?? 0;
                    }

                    return CountryCode.GetHashCode();
                }

                return LeagueId.GetHashCode();
            }

            return LeagueGroupName.GetHashCode();
        }
    }
}