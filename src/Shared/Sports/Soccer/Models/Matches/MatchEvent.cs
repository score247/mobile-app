using LiveScore.Core.Models.Matches;

namespace LiveScore.Soccer.Models.Matches
{
    public class MatchEvent : IMatchEvent
    {
        public MatchEvent(string matchId, MatchResult matchResult, TimelineEvent timeline)
        {
            MatchId = matchId;
            MatchResult = matchResult;
            Timeline = timeline;
        }

        public string MatchId { get; }

        public IMatchResult MatchResult { get; }

        public ITimelineEvent Timeline { get; }
    }
}