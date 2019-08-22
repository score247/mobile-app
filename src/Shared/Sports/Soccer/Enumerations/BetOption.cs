namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class BetOption : Enumeration
    {
        public const string Home = "home";

        public static readonly BetOption HomeType = new BetOption(1, Home);

        public const string Draw = "draw";

        public static readonly BetOption DrawType = new BetOption(2, Draw);

        public const string Away = "away";

        public static readonly BetOption AwayType = new BetOption(3, Away);

        public const string Over = "over";

        public static readonly BetOption OverType = new BetOption(4, Over);

        public const string Under = "under";

        public static readonly BetOption UnderType = new BetOption(5, Under);

        private BetOption(byte value, string displayName)
           : base(value, displayName)
        {
        }
    }
}