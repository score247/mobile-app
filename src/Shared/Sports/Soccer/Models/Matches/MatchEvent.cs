namespace LiveScore.Soccer.Models.Matches
{
    using LiveScore.Core.Models.Matches;

    public class MatchEvent
    {
        public MatchEvent(byte sportId, string matchId, MatchResult matchResult, TimelineEvent timeline)
        {
            SportId = sportId;
            MatchId = matchId;
            MatchResult = matchResult;
            Timeline = timeline;
        }

        public byte SportId { get; }

        public string MatchId { get; }

        public MatchResult MatchResult { get; }

        public TimelineEvent Timeline { get; }
    }
}