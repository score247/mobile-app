namespace LiveScore.Core.Enumerations
{
    public class PeriodTypes : Enumeration
    {
        public static readonly PeriodTypes RegularPeriod = new PeriodTypes(1, "regular_period");
        public static readonly PeriodTypes Overtime = new PeriodTypes(2, "overtime");
        public static readonly PeriodTypes Penalties = new PeriodTypes(3, "penalties");
        public static readonly PeriodTypes Pause = new PeriodTypes(4, "pause");
        public static readonly PeriodTypes AwaitingExtraTime = new PeriodTypes(5, "awaiting_extra");
        public static readonly PeriodTypes ExtraTimeHalfTime = new PeriodTypes(6, "extra_time_halftime");
        public static readonly PeriodTypes AwaitingPenalties = new PeriodTypes(7, "awaiting_penalties");

        public PeriodTypes()
        {
        }

        private PeriodTypes(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsOvertime => Value == Overtime.Value;

        public bool IsPenalties => Value == Penalties.Value;
    }
}