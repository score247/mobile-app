using LiveScore.Core.Models.Matches;

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    public class H2HMatchGrouping
    {
        public H2HMatchGrouping(IMatch match)
        {
            if (match == null)
            {
                return;
            }

            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName.ToUpperInvariant();
            LeagueSeasonId = match.LeagueSeasonId;
        }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string LeagueSeasonId { get; }

        public override bool Equals(object obj)
            => (obj is H2HMatchGrouping actualObj)
                && LeagueId == actualObj.LeagueId
                && LeagueSeasonId == actualObj.LeagueSeasonId;

        public override int GetHashCode()
        {
            if (!string.IsNullOrWhiteSpace(LeagueSeasonId))
            {
                return LeagueSeasonId.GetHashCode();
            }

            if (string.IsNullOrWhiteSpace(LeagueId))
            {
                return LeagueName?.GetHashCode() ?? 0;
            }

            return LeagueId.GetHashCode();
        }
    }
}