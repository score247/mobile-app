namespace LiveScore.Soccer.PubSubEvents.Matches
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.PubSubEvents.Matches;
    using LiveScore.Soccer.Models.Matches;

    public class MatchEventMessage : IMatchEventMessage
    {
        public MatchEventMessage(byte sportId, MatchEvent matchEvent)
        {
            SportId = sportId;
            MatchEvent = matchEvent;
        }

        public byte SportId { get; }

        public IMatchEvent MatchEvent { get; }
    }
}