namespace LiveScore.Domain.CommonValues
{
    public static class MatchStatus
    {
        //not_started – The match is scheduled to be played
        public const string NotStarted = "not_started";

        //live – The match is currently in progress
        public const string Live = "live";

        //1st_half – The match is in the first half
        public const string FirstHaft = "1st_half";

        //2nd_half – The match is in the second half
        public const string SecondHaft = "2nd_half";

        //overtime – The match is in overtime
        public const string Overtime = "overtime";

        //1st_extra – The match is in the first extra period
        public const string FirstHaftExtra = "1st_extra";

        //2nd_extra – The match is in the second extra period
        public const string SecondHaftExtra = "2nd_extra";

        //awaiting_penalties – Waiting for announcement of penalties
        public const string AwaitingPenalties = "awaiting_penalties";

        //penalties – Penalties are ongoing
        public const string Penalties = "penalties";

        //pause – The match is paused
        public const string Pause = "pause";

        //awaiting_extra_time – Waiting on referee to announce extra time
        public const string AwaitingExtraTime = "awaiting_extra_time";

        //interrupted – The match has been interrupted
        public const string Interrupted = "not_stainterruptedrted";

        //abandoned – The match has been abandoned
        public const string Abandoned = "abandoned";

        //postponed – The match has been postponed to a future date
        public const string Postponed = "postponed";

        //delayed – The match has been temporarily delayed and will be continued
        public const string Delayed = "delayed";

        //cancelled – The match has been canceled and will not be played
        public const string Cancelled = "cancelled";

        //ended – The match is over
        public const string Ended = "ended";

        //closed – The match results have been confirmed
        public const string Closed = "closed";

        //halftime – The match is in halftime
        public const string Halftime = "halftime";

        //full-time – The match has ended
        public const string FullTime = "full-time";

        //extra_time – Extra time has been added
        public const string ExtraTime = "extra_time";

        //aet – The match has ended after extra time
        public const string EndedExtraTime = "aet";

        //ap – The match has ended after penalties
        public const string EndedAfterPenalties = "ap";

    }
}
