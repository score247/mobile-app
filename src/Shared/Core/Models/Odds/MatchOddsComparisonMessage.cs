namespace LiveScore.Core.Models.Odds
{
    using System.Collections.Generic;

    public interface IOddsComparisonMessage
    {
        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}
