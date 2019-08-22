namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class BetType : Enumeration
    {
        public static readonly BetType OneXTwo = new BetType(1, "OneXTwo");
        public static readonly BetType OverUnder = new BetType(2, "OverUnder");
        public static readonly BetType AsianHDP = new BetType(3, "AsianHDP");

        private BetType(byte value, string displayName)
           : base(value, displayName)
        {
        }

        public BetType()
        {
        }
    }
}