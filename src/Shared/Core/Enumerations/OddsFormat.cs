namespace LiveScore.Core.Enumerations
{
    public class OddsFormat : Enumeration
    {
        public static readonly OddsFormat Decimal = new OddsFormat(1, "dec");

        public OddsFormat()
        {
        }

        public OddsFormat(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
