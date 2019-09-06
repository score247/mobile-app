namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using Odds;

    public interface IMatchOdds
    {
        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}