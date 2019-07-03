namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class BetType : Enumeration
    {
        public const string OneXTwo = "1x2";

        public static readonly BetType OneXTwoType = new BetType(OneXTwo, nameof(OneXTwo));

        public const string AsianHDP = "asian_handicap";

        public static readonly BetType AsianHDPType = new BetType(AsianHDP, nameof(AsianHDP));

        public const string OverUnder = "over_under";

        public static readonly BetType OverUnderType = new BetType(OverUnder, nameof(OverUnder));

        private BetType(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
