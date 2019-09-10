namespace LiveScore.Core.Models.Odds
{
    using Enumerations;

    public class BetOptionOdds : Entity<int, string>
    {
        public BetOptionOdds(int id, string name, string type, decimal liveOdds, decimal openingOdds, string optionValue, string openingOptionValue, OddsTrend oddsTrend)
        {
            Id = id;
            Name = name;
            Type = type;
            LiveOdds = liveOdds;
            OpeningOdds = openingOdds;
            OptionValue = optionValue;
            OpeningOptionValue = openingOptionValue;
            OddsTrend = oddsTrend;
        }

        public string Type { get; }

        public decimal LiveOdds { get; }

        public decimal OpeningOdds { get; }

        public string OptionValue { get; }

        public string OpeningOptionValue { get; }

        public OddsTrend OddsTrend { get; }
    }
}