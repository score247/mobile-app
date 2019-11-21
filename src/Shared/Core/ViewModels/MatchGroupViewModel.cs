using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels
{
    public class MatchGroupViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly int currentSportId;

        public MatchGroupViewModel(IMatch match, Func<string, string> buildFlagUrl, INavigationService navigationService, int currentSportId)
        {
            if (match == null)
            {
                IsBusy = true;
                return;
            }

            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName.ToUpperInvariant();
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
            CountryFlag = buildFlagUrl(match.CountryCode);
            CountryCode = match.CountryCode;
            LeagueOrder = match.LeagueOrder;
            Match = match;

            this.navigationService = navigationService;
            this.currentSportId = currentSportId;
            TapLeagueCommand = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

        private IMatch Match { get; }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string EventDate { get; }

        public string CountryCode { get; }

        public string CountryFlag { get; }

        public int LeagueOrder { get; }

        public DelegateAsyncCommand TapLeagueCommand { get; }

        private async Task OnTapLeagueAsync()
        {
            var parameters = new NavigationParameters
            {
                { "LeagueId", LeagueId }
            };

            await navigationService
                .NavigateAsync("LeagueDetailView" + currentSportId, parameters)
                .ConfigureAwait(false);
        }

        public override bool Equals(object obj)
            => (obj is MatchGroupViewModel actualObj) && LeagueName == actualObj.LeagueName && EventDate == actualObj.EventDate;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(LeagueName))
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

            return LeagueName.GetHashCode();
        }
    }
}