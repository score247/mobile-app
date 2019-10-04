using LiveScore.Soccer.Models.Odds;
using Prism.Events;

namespace LiveScore.Core.PubSubEvents.Odds
{
    public class OddsMovementPubSubEvent : PubSubEvent<OddsMovementMessage>
    { }
}