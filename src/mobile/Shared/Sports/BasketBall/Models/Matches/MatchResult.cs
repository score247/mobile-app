namespace LiveScore.BasketBall.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public class MatchResult : IMatchResult
    {
        public MatchStatus Status { get; set; }

        public IEnumerable<int> HomeScores { get; set; }

        public IEnumerable<int> AwayScores { get; set; }
    }
}