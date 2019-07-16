namespace LiveScore.Core.Enumerations
{
    public class OddsFormat : Enumeration
    {
        public static readonly OddsFormat Decimal = new OddsFormat("dec", nameof(Decimal));

        public OddsFormat()
        {
        }

        public OddsFormat(string value, string displayName)
            : base(value, displayName)
        {
        }

        public OddsFormat(string value)
            : base(value, value)
        {
        }
    }
}
