namespace LiveScore.Core.Models.Odds
{
    using LiveScore.Core.Enumerations;

    public interface IBetOptionOdds : IEntity<int, string>
    {
        string Type { get;  }

        decimal LiveOdds { get; }

        decimal OpeningOdds { get; }

        string OptionValue { get; }

        OddsTrend OddsTrend { get; }
    }

    public class BetOptionOdds : Entity<int, string>, IBetOptionOdds
    {
        public string Type { get; set; }

        public decimal LiveOdds { get; set; }

        public decimal OpeningOdds { get; set; }

        public string OptionValue { get; set; }

        public OddsTrend OddsTrend { get; set; }
    }
}
