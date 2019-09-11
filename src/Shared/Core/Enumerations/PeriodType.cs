namespace LiveScore.Core.Enumerations
{
    using MessagePack;

    [MessagePackObject]
    public class PeriodType : Enumeration
    {
        public static readonly PeriodType RegularPeriod = new PeriodType(1, "regular_period");
        public static readonly PeriodType Overtime = new PeriodType(2, "overtime");
        public static readonly PeriodType Penalties = new PeriodType(3, "penalties");
        public static readonly PeriodType Pause = new PeriodType(4, "pause");
        public static readonly PeriodType AwaitingExtraTime = new PeriodType(5, "awaiting_extra");
        public static readonly PeriodType ExtraTimeHalfTime = new PeriodType(6, "extra_time_halftime");
        public static readonly PeriodType AwaitingPenalties = new PeriodType(7, "awaiting_penalties");

        public PeriodType()
        {
        }

        private PeriodType(byte value, string displayName)
            : base(value, displayName)
        {
        }

        [IgnoreMember]
        public bool IsOvertime => Value == Overtime.Value;

        [IgnoreMember]
        public bool IsPenalties => Value == Penalties.Value;
    }
}