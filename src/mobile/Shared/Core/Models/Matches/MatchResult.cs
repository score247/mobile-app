namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;

    public interface IMatchResult
    {
        MatchStatus Status { get; }

        IEnumerable<int> HomeScores { get; }

        IEnumerable<int> AwayScores { get; }
    }
}