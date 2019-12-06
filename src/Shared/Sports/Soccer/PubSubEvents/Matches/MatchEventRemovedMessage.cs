using LiveScore.Core.PubSubEvents.Matches;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class MatchEventRemovedMessage: IMatchEventRemovedMessage
    {
        public MatchEventRemovedMessage(string matchId, string[] timelineIds)
        {
            MatchId = matchId;
            TimelineIds = timelineIds;
        }

        public string MatchId { get; }

        public string[] TimelineIds { get; }
    }
}
