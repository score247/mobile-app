namespace LiveScore.Domain.CommonValues
{
    public static class EventStatus
    {
        //not_started – The match is scheduled to be played
        public const string NotStarted = "not_started";

        //live – The match is currently in progress
        public const string Live = "live";

        //postponed – The match has been postponed to a future date
        public const string Postponed = "postponed";

        //delayed – The match has been temporarily delayed and will be continued
        public const string Delayed = "delayed";

        //start_delayed – The start of the match has been temporarily delayed
        public const string StartDelayed = "start_delayed";

        //canceled – The match has been canceled and will not be played
        public const string Canceled = "canceled";

        //ended – The match is over
        public const string Ended = "ended";

        //closed – The match results have been confirmed
        public const string Closed = "closed";

        //abandoned – The match has been abandoned
        public const string Abandoned = "abandoned";
    }
}