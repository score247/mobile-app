namespace LiveScore.Core.Events
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;
    using Prism.Events;

    public class OddsComparisonPubSubEvent : PubSubEvent<IOddsComparisonMessage>
    { }

    public interface IOddsComparisonMessage
    {
        byte SportId { get; }

        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}