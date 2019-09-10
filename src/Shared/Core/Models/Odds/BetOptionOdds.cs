namespace LiveScore.Core.Models.Odds
{
    using Enumerations;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class BetOptionOdds
    {
        public string Type { get; set; }

        public decimal LiveOdds { get; set; }

        public decimal OpeningOdds { get; set; }

        public string OptionValue { get; set; }

        public string OpeningOptionValue { get; set; }

        public OddsTrend OddsTrend { get; set; }
    }
}