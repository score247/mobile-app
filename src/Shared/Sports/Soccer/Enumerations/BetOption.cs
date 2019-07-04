namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class BetOption : Enumeration
    {
        public const string Home = "home";

        public static readonly BetOption HomeType = new BetOption(Home, nameof(Home));

        public const string Draw = "draw";

        public static readonly BetOption DrawType = new BetOption(Draw, nameof(Draw));

        public const string Away = "away";

        public static readonly BetOption AwayType = new BetOption(Away, nameof(Away));

        private BetOption(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
