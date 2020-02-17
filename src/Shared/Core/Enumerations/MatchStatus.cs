using MessagePack;

namespace LiveScore.Core.Enumerations
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchStatus : Enumeration
    {
        /// <summary>
        /// <para>value: 1</para>
        /// <para>name: not_started</para>
        /// </summary>
        public static readonly MatchStatus NotStarted = new MatchStatus(1, "not_started");

        /// <summary>
        /// <para>value: 2</para>
        /// <para>name: postponed</para>
        /// </summary>
        public static readonly MatchStatus Postponed = new MatchStatus(2, "postponed");

        /// <summary>
        /// <para>value: 3</para>
        /// <para>name: start_delayed</para>
        /// </summary>
        public static readonly MatchStatus StartDelayed = new MatchStatus(3, "start_delayed");

        /// <summary>
        /// <para>value: 4</para>
        /// <para>name: cancelled</para>
        /// </summary>
        public static readonly MatchStatus Cancelled = new MatchStatus(4, "cancelled");

        /// <summary>
        /// <para>value: 5</para>
        /// <para>name: live</para>
        /// </summary>
        public static readonly MatchStatus Live = new MatchStatus(5, "live");

        /// <summary>
        /// <para>value: 6</para>
        /// <para>name: 1st_half</para>
        /// </summary>
        public static readonly MatchStatus FirstHalf = new MatchStatus(6, "1st_half");

        /// <summary>
        /// <para>value: 7</para>
        /// <para>name: 2nd_half</para>
        /// </summary>
        public static readonly MatchStatus SecondHalf = new MatchStatus(7, "2nd_half");

        /// <summary>
        /// <para>value: 8</para>
        /// <para>name: overtime</para>
        /// </summary>
        public static readonly MatchStatus Overtime = new MatchStatus(8, "overtime");

        /// <summary>
        /// <para>value: 9</para>
        /// <para>name: 1st_extra</para>
        /// </summary>
        public static readonly MatchStatus FirstHalfExtra = new MatchStatus(9, "1st_extra");

        /// <summary>
        /// <para>value: 10</para>
        /// <para>name: 2nd_extra</para>
        /// </summary>
        public static readonly MatchStatus SecondHalfExtra = new MatchStatus(10, "2nd_extra");

        /// <summary>
        /// <para>value: 11</para>
        /// <para>name: awaiting_penalties</para>
        /// </summary>
        public static readonly MatchStatus AwaitingPenalties = new MatchStatus(11, "awaiting_penalties");

        /// <summary>
        /// <para>value: 12</para>
        /// <para>name: penalties</para>
        /// </summary>
        public static readonly MatchStatus Penalties = new MatchStatus(12, "penalties");

        /// <summary>
        /// <para>value: 13</para>
        /// <para>name: pause</para>
        /// </summary>
        public static readonly MatchStatus Pause = new MatchStatus(13, "pause");

        /// <summary>
        /// <para>value: 14</para>
        /// <para>name: awaiting_extra_time</para>
        /// </summary>
        public static readonly MatchStatus AwaitingExtraTime = new MatchStatus(14, "awaiting_extra_time");

        /// <summary>
        /// <para>value: 15</para>
        /// <para>name: interrupted</para>
        /// </summary>
        public static readonly MatchStatus Interrupted = new MatchStatus(15, "interrupted");

        /// <summary>
        /// <para>value: 16</para>
        /// <para>name: halftime</para>
        /// </summary>
        public static readonly MatchStatus Halftime = new MatchStatus(16, "halftime");

        /// <summary>
        /// <para>value: 17</para>
        /// <para>name: full-time</para>
        /// </summary>
        public static readonly MatchStatus FullTime = new MatchStatus(17, "full-time");

        /// <summary>
        /// <para>value: 18</para>
        /// <para>name: extra_time</para>
        /// </summary>
        public static readonly MatchStatus ExtraTime = new MatchStatus(18, "extra_time");

        /// <summary>
        /// <para>value: 19</para>
        /// <para>name: delayed</para>
        /// </summary>
        public static readonly MatchStatus Delayed = new MatchStatus(19, "delayed");

        /// <summary>
        /// <para>value: 20</para>
        /// <para>name: abandoned</para>
        /// </summary>
        public static readonly MatchStatus Abandoned = new MatchStatus(20, "abandoned");

        /// <summary>
        /// <para>value: 21</para>
        /// <para>name: extra_time_halftime</para>
        /// </summary>
        public static readonly MatchStatus ExtraTimeHalfTime = new MatchStatus(21, "extra_time_halftime");

        /// <summary>
        /// <para>value: 22</para>
        /// <para>name: ended</para>
        /// </summary>
        public static readonly MatchStatus Ended = new MatchStatus(22, "ended");

        /// <summary>
        /// <para>value: 23</para>
        /// <para>name: closed</para>
        /// </summary>
        public static readonly MatchStatus Closed = new MatchStatus(23, "closed");

        /// <summary>
        /// <para>value: 24</para>
        /// <para>name: aet</para>
        /// </summary>
        public static readonly MatchStatus EndedExtraTime = new MatchStatus(24, "aet");

        /// <summary>
        /// <para>value: 25</para>
        /// <para>name: ap</para>
        /// </summary>
        public static readonly MatchStatus EndedAfterPenalties = new MatchStatus(25, "ap");

        /// <summary>
        /// <para>value: 26</para>
        /// <para>name: started</para>
        /// </summary>
        public static readonly MatchStatus Started = new MatchStatus(26, "started");

        public MatchStatus()
        {
        }

        public MatchStatus(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public MatchStatus(byte value)
            : base(value, value.ToString())
        {
        }

        [IgnoreMember]
        public bool IsPreMatch => this == NotStarted || this == Postponed || this == Cancelled || this == StartDelayed;

        [IgnoreMember]
        public bool IsNotStarted => this == NotStarted;

        [IgnoreMember]
        public bool IsLive => this == Live;

        [IgnoreMember]
        public bool IsClosed => this == Closed;

        [IgnoreMember]
        public bool IsEnded => this == Ended;

        [IgnoreMember]
        public bool IsFirstHalf => this == FirstHalf;

        [IgnoreMember]
        public bool IsSecondHalf => this == SecondHalf;

        [IgnoreMember]
        public bool IsAfterExtraTime => this == EndedExtraTime;

        [IgnoreMember]
        public bool IsInPenalties => this == Penalties;

        [IgnoreMember]
        public bool IsAfterPenalties => this == EndedAfterPenalties;

        [IgnoreMember]
        public bool IsFirstHalfExtra => this == FirstHalfExtra;

        [IgnoreMember]
        public bool IsSecondHalfExtra => this == SecondHalfExtra;

        [IgnoreMember]
        public bool NotShowScore => this == NotStarted || this == Cancelled || this == Postponed || this == StartDelayed;

        [IgnoreMember]
        public bool ShowScore => !NotShowScore;

        [IgnoreMember]
        public bool IsInExtraTime => this == FirstHalfExtra || this == SecondHalfExtra || this == ExtraTimeHalfTime;
    }
}