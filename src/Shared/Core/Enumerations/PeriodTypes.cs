namespace LiveScore.Core.Enumerations
{
    public class PeriodTypes : Enumeration
    {
        public const string RegularPeriod = "regular_period";
        public const string Overtime = "overtime";
        public const string Penalties = "penalties";

        public static readonly PeriodTypes RegularPeriodType = new PeriodTypes(RegularPeriod, nameof(RegularPeriod));
        public static readonly PeriodTypes OvertimeType = new PeriodTypes(Overtime, nameof(Overtime));
        public static readonly PeriodTypes PenaltiesType = new PeriodTypes(Penalties, nameof(Penalties));

        public PeriodTypes()
        {
        }

        private PeriodTypes(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}