namespace LiveScore.Core.PubSubEvents.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;
    using Prism.Events;

    public class OddsComparisonPubSubEvent : PubSubEvent<IOddsComparisonMessage>
    { }
}