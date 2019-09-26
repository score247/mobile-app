namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchResult : IMatchResult
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public MatchResult(
            MatchStatus matchStatus,
            MatchStatus eventStatus,
            byte period,
            IEnumerable<MatchPeriod> matchPeriods,
            byte matchTime,
            string winnerId,
            byte homeScore,
            byte awayScore,
            byte aggregateHomeScore,
            byte aggregateAwayScore,
            string aggregateWinnerId)
        {
            MatchStatus = matchStatus;
            EventStatus = eventStatus;
            Period = period;
            MatchPeriods = matchPeriods;
            MatchTime = matchTime;
            WinnerId = winnerId;
            HomeScore = homeScore;
            AwayScore = awayScore;
            AggregateHomeScore = aggregateHomeScore;
            AggregateAwayScore = aggregateAwayScore;
            AggregateWinnerId = aggregateWinnerId;
        }

        private MatchResult()
        {
        }

        internal MatchResult(MatchStatus eventStatus, MatchStatus matchStatus)
        {
            this.EventStatus = eventStatus;
            this.MatchStatus = matchStatus;
        }

        internal static MatchResult MatchResult_EventNotStarted => new MatchResult
        {
            EventStatus = MatchStatus.NotStarted
        };

#pragma warning restore S107 // Methods should not have too many parameters

        public MatchStatus MatchStatus { get; private set; }

        public MatchStatus EventStatus { get; private set; }

        public byte Period { get; private set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; private set; }

        public byte MatchTime { get; private set; }

        public string WinnerId { get; private set; }

        public byte HomeScore { get; private set; }

        public byte AwayScore { get; private set; }

        public byte AggregateHomeScore { get; private set; }

        public byte AggregateAwayScore { get; private set; }

        public string AggregateWinnerId { get; private set; }

        public void UpdateMatchTime(byte matchTime)
        {
            MatchTime = matchTime;
        }
    }
}