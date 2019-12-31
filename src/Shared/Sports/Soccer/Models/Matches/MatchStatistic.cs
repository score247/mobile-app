using LiveScore.Soccer.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchStatistic
    {
        public MatchStatistic(string matchId)
        {
            MatchId = matchId;
        }

        [SerializationConstructor]
        public MatchStatistic(
            string matchId,
            TeamStatistic homeStatistic,
            TeamStatistic awayStatistic)
        {
            MatchId = matchId;
            HomeStatistic = homeStatistic;
            AwayStatistic = awayStatistic;
        }

        public string MatchId { get; }

        public TeamStatistic HomeStatistic { get; }

        public TeamStatistic AwayStatistic { get; }
    }
}