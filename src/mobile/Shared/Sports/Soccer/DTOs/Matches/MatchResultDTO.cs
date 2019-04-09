namespace LiveScore.Soccer.DTOs.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;

    public class MatchResultDTO
    {
        public MatchStatus Status { get; set; }

        public IEnumerable<int> HomeScores { get; set; }

        public IEnumerable<int> AwayScores { get; set; }
    }
}