namespace LiveScore.Core.Enumerations
{
    public class PeriodTypes : Enumeration
    {
        public const string RegularPeriod = "regular_period";
        public const string Overtime = "overtime";
        public const string Penalties = "penalties";
        public const string Pause = "pause";
        public const string AwaitingExtraTime = "awaiting_extra";
        public const string ExtraTimeHalfTime = "extra_time_halftime";
        public const string AwaitingPenalties = "awaiting_penalties";

        public static readonly PeriodTypes RegularPeriodType = new PeriodTypes(RegularPeriod, nameof(RegularPeriod));
        public static readonly PeriodTypes OvertimeType = new PeriodTypes(Overtime, nameof(Overtime));
        public static readonly PeriodTypes PenaltiesType = new PeriodTypes(Penalties, nameof(Penalties));
        public static readonly PeriodTypes PauseType = new PeriodTypes(Pause, nameof(Pause));
        public static readonly PeriodTypes AwaitingExtraTimeType = new PeriodTypes(AwaitingExtraTime, nameof(AwaitingExtraTime));
        public static readonly PeriodTypes ExtraTimeHalfTimeType = new PeriodTypes(ExtraTimeHalfTime, nameof(ExtraTimeHalfTime));
        public static readonly PeriodTypes AwaitingPenaltiesType = new PeriodTypes(AwaitingPenalties, nameof(AwaitingPenalties));

        public PeriodTypes()
        {
        }

        private PeriodTypes(string value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsOvertime => Value == Overtime;

        public bool IsPenalties => Value == Penalties;
    }
}