namespace LiveScore.Core.Models.Odds
{
    using Enumerations;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class BetOptionOdds
    {
        public BetOptionOdds(string type, decimal liveOdds, decimal openingOdds, string optionValue, string openingOptionValue, OddsTrend oddsTrend)
        {
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