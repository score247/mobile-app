namespace LiveScore.Core.Enumerations
{
    public class SportType : Enumeration
    {
        public static readonly SportType Soccer = new SportType(1, nameof(Soccer));
        public static readonly SportType Basketball = new SportType(2, nameof(Basketball));
        public static readonly SportType ESport = new SportType(3, nameof(ESport));

        public SportType()
        {
        }

        private SportType(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsSoccer() => this == Soccer;
    }
}