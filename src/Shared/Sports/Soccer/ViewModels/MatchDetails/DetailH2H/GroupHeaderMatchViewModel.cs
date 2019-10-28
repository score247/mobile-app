using System;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H
{
    public class GroupHeaderMatchViewModel : BaseViewModel
    {
        public GroupHeaderMatchViewModel(IMatch match, Func<string, string> buildFlagUrl)
        {
            if (match == null)
            {
                IsBusy = true;
                return;
            }

            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName;
            CountryFlag = buildFlagUrl(match.CountryCode);
            CountryCode = match.CountryCode;
            LeagueOrder = match.LeagueOrder;
            LeagueSeasonId = match.LeagueSeasonId;
            
            Match = match;
        }

        private IMatch Match { get; }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string CountryCode { get; }

        public string CountryFlag { get; }

        public int LeagueOrder { get; }

        public string LeagueSeasonId { get; }

        public override bool Equals(object obj)
            => (obj is GroupHeaderMatchViewModel actualObj) && LeagueId == actualObj.LeagueId && LeagueSeasonId == actualObj.LeagueSeasonId;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(LeagueSeasonId))
            {
                if (string.IsNullOrWhiteSpace(LeagueId))
                {
                    return LeagueName?.GetHashCode() ?? 0;
                }

                return LeagueId.GetHashCode();
            }

            return LeagueSeasonId.GetHashCode();
        }
    }
}
