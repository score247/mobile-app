namespace LiveScore.Core.Enumerations
{
    public class SportTypes : Enumeration
    {
        public static readonly SportTypes Soccer = new SportTypes("1", nameof(Soccer));
        public static readonly SportTypes Basketball = new SportTypes("2", nameof(Basketball));
        public static readonly SportTypes ESport = new SportTypes("3", nameof(ESport));

        public SportTypes()
        {
        }

        private SportTypes(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}