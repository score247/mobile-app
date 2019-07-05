namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class OddsTrend : Enumeration
    {
        public const string Up = "up";

        public static readonly OddsTrend UpTrend = new OddsTrend(Up, nameof(Up));

        public const string Down = "down";

        public static readonly OddsTrend DownTrend = new OddsTrend(Down, nameof(Down));

        public const string Neutral = "neutral";

        public static readonly OddsTrend NeutralTrend = new OddsTrend(Neutral, nameof(Neutral));

        private OddsTrend(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
