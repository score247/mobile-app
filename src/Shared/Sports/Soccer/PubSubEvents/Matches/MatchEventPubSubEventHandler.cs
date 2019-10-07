using LiveScore.Core.PubSubEvents.Matches;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class MatchEventPubSubEventHandler 
        : BasePubSubEventHandler<MatchEventMessage, MatchEventPubSubEvent>
    {
        public MatchEventPubSubEventHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public override string HubMethod => "MatchEvent";

        protected override void Publish(MatchEventMessage message, MatchEventPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }
}