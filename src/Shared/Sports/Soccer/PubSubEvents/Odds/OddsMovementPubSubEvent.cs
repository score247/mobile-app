namespace LiveScore.Core.PubSubEvents.Odds
{
    using LiveScore.Soccer.Models.Odds;
    using Prism.Events;

    public class OddsMovementPubSubEvent : PubSubEvent<OddsMovementMessage>
    { }
}