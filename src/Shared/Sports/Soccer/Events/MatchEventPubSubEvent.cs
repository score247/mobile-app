namespace LiveScore.Soccer.Events
{
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Matches;
    using Prism.Events;

    public class MatchEventMessage : IMatchEventMessage
    {
        public const string HubMethod = "MatchEvent";

        public MatchEventMessage(byte sportId, IMatchEvent matchEvent)
        {
            SportId = sportId;
            MatchEvent = matchEvent;
        }

        public byte SportId { get; private set; }

        public IMatchEvent MatchEvent { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<MatchEventPubSubEvent>().Publish(data as MatchEventMessage);
    }
}