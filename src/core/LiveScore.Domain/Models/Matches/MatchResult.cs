namespace LiveScore.Domain.Models.Matches
{
    using System.Collections.Generic;

    public interface IMatchResult
    {
        string Status { get; }

        IEnumerable<int> HomeScores { get; }

        IEnumerable<int> AwayScores { get; }
    }

    public class MatchResult : IMatchResult
    {
        public string Status { get; set; }

        public IEnumerable<int> HomeScores { get; set; }

        public IEnumerable<int> AwayScores { get; set; }
    }
}