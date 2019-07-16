namespace LiveScore.Core.Models.Odds
{
    using LiveScore.Core.Enumerations;

    public class BetOptionOdds : Entity<int, string>
    {
        public string Type { get; set; }

        public decimal LiveOdds { get; set; }

        public decimal OpeningOdds { get; set; }

        public string OptionValue { get; set; }

        public string OpeningOptionValue { get; set; }

        public OddsTrend OddsTrend { get; set; }
    }
}
