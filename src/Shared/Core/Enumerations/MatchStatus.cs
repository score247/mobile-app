namespace LiveScore.Core.Enumerations
{
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchStatus : Enumeration
    {
        //not_started – The match is scheduled to be played
        public const string NotStarted = "not_started";

        public static readonly MatchStatus NotStartedStatus = new MatchStatus(NotStarted, nameof(NotStarted));

        //live – The match is currently in progress
        public const string Live = "live";

        public static readonly MatchStatus LiveStatus = new MatchStatus(Live, nameof(Live));

        //1st_half – The match is in the first half
        public const string FirstHaft = "1st_half";

        public static readonly MatchStatus FirstHaftStatus = new MatchStatus(FirstHaft, nameof(FirstHaft));

        //2nd_half – The match is in the second half
        public const string SecondHaft = "2nd_half";

        public static readonly MatchStatus SecondHaftStatus = new MatchStatus(SecondHaft, nameof(SecondHaft));

        //overtime – The match is in overtime
        public const string Overtime = "overtime";

        public static readonly MatchStatus OvertimeStatus = new MatchStatus(Overtime, nameof(Overtime));

        //1st_extra – The match is in the first extra period
        public const string FirstHaftExtra = "1st_extra";

        public static readonly MatchStatus FirstHaftExtraStatus = new MatchStatus(FirstHaftExtra, nameof(FirstHaftExtra));

        //2nd_extra – The match is in the second extra period
        public const string SecondHaftExtra = "2nd_extra";

        public static readonly MatchStatus SecondHaftExtraStatus = new MatchStatus(SecondHaftExtra, nameof(SecondHaftExtra));

        //awaiting_penalties – Waiting for announcement of penalties
        public const string AwaitingPenalties = "awaiting_penalties";

        public static readonly MatchStatus AwaitingPenaltiesStatus = new MatchStatus(AwaitingPenalties, nameof(AwaitingPenalties));

        //penalties – Penalties are ongoing
        public const string Penalties = "penalties";

        public static readonly MatchStatus PenaltiesStatus = new MatchStatus(Penalties, nameof(Penalties));

        //pause – The match is paused
        public const string Pause = "pause";

        public static readonly MatchStatus PauseStatus = new MatchStatus(Pause, nameof(Pause));

        //awaiting_extra_time – Waiting on referee to announce extra time
        public const string AwaitingExtraTime = "awaiting_extra_time";

        public static readonly MatchStatus AwaitingExtraTimeStatus = new MatchStatus(AwaitingExtraTime, nameof(AwaitingExtraTime));

        //interrupted – The match has been interrupted
        public const string Interrupted = "interrupted";

        public static readonly MatchStatus InterruptedStatus = new MatchStatus(Interrupted, nameof(Interrupted));

        //abandoned – The match has been abandoned
        public const string Abandoned = "abandoned";

        public static readonly MatchStatus AbandonedStatus = new MatchStatus(Abandoned, nameof(Abandoned));

        //postponed – The match has been postponed to a future date
        public const string Postponed = "postponed";

        public static readonly MatchStatus PostponedStatus = new MatchStatus(Postponed, nameof(Postponed));

        //delayed – The match has been temporarily delayed and will be continued
        public const string Delayed = "delayed";

        public static readonly MatchStatus DelayedStatus = new MatchStatus(Delayed, nameof(Delayed));

        //ended – The match is over
        public const string Ended = "ended";

        public static readonly MatchStatus EndedStatus = new MatchStatus(Ended, nameof(Ended));

        //closed – The match results have been confirmed
        public const string Closed = "closed";

        public static readonly MatchStatus ClosedStatus = new MatchStatus(Closed, nameof(Closed));

        //halftime – The match is in halftime
        public const string Halftime = "halftime";

        public static readonly MatchStatus HalftimeStatus = new MatchStatus(Halftime, nameof(Halftime));

        //full-time – The match has ended
        public const string FullTime = "full-time";

        public static readonly MatchStatus FullTimeStatus = new MatchStatus(FullTime, nameof(FullTime));

        //extra_time – Extra time has been added
        public const string ExtraTime = "extra_time";

        public static readonly MatchStatus ExtraTimeStatus = new MatchStatus(ExtraTime, nameof(ExtraTime));

        //aet – The match has ended after extra time
        public const string EndedExtraTime = "aet";

        public static readonly MatchStatus EndedExtraTimeStatus = new MatchStatus(EndedExtraTime, nameof(EndedExtraTime));

        //ap – The match has ended after penalties
        public const string EndedAfterPenalties = "ap";

        public static readonly MatchStatus EndedAfterPenaltiesStatus = new MatchStatus(EndedAfterPenalties, nameof(EndedAfterPenalties));

        //start_delayed – The start of the match has been temporarily delayed
        public const string StartDelayed = "start_delayed";

        public static readonly MatchStatus StartDelayedStatus = new MatchStatus(StartDelayed, nameof(StartDelayed));

        //canceled – The match has been canceled and will not be played
        public const string Cancelled = "cancelled";

        public static readonly MatchStatus CancelledStatus = new MatchStatus(Cancelled, nameof(Cancelled));

        public MatchStatus()
        {
        }

        private MatchStatus(string value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsPreMatch => Value == NotStarted || Value == Postponed || Value == Cancelled || Value == StartDelayed;

        public bool IsNotStarted => Value == NotStarted;

        public bool IsLive => Value == Live;

        public bool IsClosed => Value == Closed;

        public bool IsEnded => Value == Ended;

        public bool IsFirstHalf => Value == FirstHaft;

        public bool IsSecondHalf => Value == SecondHaft;

        public bool IsAfterExtraTime => Value == EndedExtraTime;

        public bool IsAfterPenalties => Value == EndedAfterPenalties;

        public bool IsFirstHalfExtra => Value == FirstHaftExtra;

        public bool IsSecondHalfExtra => Value == SecondHaftExtra;

        public bool NotShowScore => Value == NotStarted || Value == Cancelled || Value == Postponed || Value == StartDelayed;

        public bool ShowScore => !NotShowScore;
    }
}