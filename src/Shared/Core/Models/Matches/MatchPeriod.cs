namespace LiveScore.Core.Models.Matches
{
    using LiveScore.Core.Enumerations;

    public class MatchPeriod
    {
        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public PeriodTypes PeriodType { get; set; }

        public int Number { get; set; }
    }
}
