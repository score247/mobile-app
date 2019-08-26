namespace LiveScore.Core.PubSubEvents.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;

    public interface IOddsComparisonMessage
    {
        byte SportId { get; }

        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}