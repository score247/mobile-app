namespace LiveScore.Soccer.PubSubEvents.Matches
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.PubSubEvents.Matches;
    using LiveScore.Soccer.Models.Matches;
    using Prism.Events;

    public class MatchEventMessage : IMatchEventMessage
    {
        public const string HubMethod = "MatchEvent";

        public MatchEventMessage(byte sportId, MatchEvent matchEvent)
        {
            SportId = sportId;
            MatchEvent = matchEvent;
        }

        public byte SportId { get; }

        public IMatchEvent MatchEvent { get; }

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<MatchEventPubSubEvent>().Publish(data as MatchEventMessage);
    }
}