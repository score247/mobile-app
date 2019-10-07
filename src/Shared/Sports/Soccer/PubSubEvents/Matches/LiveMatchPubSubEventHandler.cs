using LiveScore.Core.PubSubEvents.Matches;
using Newtonsoft.Json;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Matches
{
    public class LiveMatchPubSubEventHandler 
        : BasePubSubEventHandler<LiveMatchMessage, LiveMatchPubSubEvent>
    {
        public LiveMatchPubSubEventHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public override string HubMethod => "LiveMatches";

        protected override void Publish(LiveMatchMessage message, LiveMatchPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }
}