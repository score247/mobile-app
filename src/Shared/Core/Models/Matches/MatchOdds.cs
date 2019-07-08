namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;

    public interface IMatchOdds
    {
        string MatchId { get; }

        IEnumerable<IBetTypeOdds> BetTypeOddsList { get; }
    }
}
