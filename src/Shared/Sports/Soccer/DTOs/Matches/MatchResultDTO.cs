namespace LiveScore.Soccer.DTOs.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public class MatchResultDto
    {
        public MatchStatus MatchStatus { get; set; }

        public MatchStatus EventStatus { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string WinnerId { get; set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; set; }
    }
}