﻿using LiveScore.Soccer.Models.Odds;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Odds
{
    public class OddsMovementPubSubEventHandler
        : BasePubSubEventHandler<OddsMovementMessage, OddsMovementPubSubEvent>
    {
        public OddsMovementPubSubEventHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public override string HubMethod => "OddsMovement";

        protected override void Publish(OddsMovementMessage message, OddsMovementPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }

    public class OddsMovementPubSubEvent : PubSubEvent<OddsMovementMessage>
    { }
}