using LiveScore.Core.PubSubEvents.Matches;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class MatchEventRemovedPubSubEventHandler
        : BasePubSubEventHandler<MatchEventRemovedMessage, MatchEventRemovedPubSubEvent>
    {
        public MatchEventRemovedPubSubEventHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
  
        public override string HubMethod => "RemovedEvent";

        protected override void Publish(MatchEventRemovedMessage message, MatchEventRemovedPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }
}
