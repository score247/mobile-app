﻿using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Odds
{
    public class OddsComparisonPubSubEventHandler : BasePubSubEventHandler<OddsComparisonMessage, OddsComparisonPubSubEvent>
    {
        public OddsComparisonPubSubEventHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public override string HubMethod => "OddsComparison";

        protected override void Publish(OddsComparisonMessage message, OddsComparisonPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }

    public class OddsComparisonPubSubEvent : PubSubEvent<OddsComparisonMessage>
    { }
}