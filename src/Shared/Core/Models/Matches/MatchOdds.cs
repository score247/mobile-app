namespace LiveScore.Core.Models.Matches
{
    using LiveScore.Core.Models.Odds;
    using System.Collections.Generic;

    public interface IMatchOdds
    {
        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}
