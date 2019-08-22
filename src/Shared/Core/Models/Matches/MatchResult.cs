namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;

    public interface IMatchResult
    {
        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        byte HomeScore { get; }

        byte AwayScore { get; }

        byte Period { get; }

        byte MatchTime { get; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        string WinnerId { get; }

        byte AggregateHomeScore { get; }

        byte AggregateAwayScore { get; }

        string AggregateWinnerId { get; }

        void UpdateMatchTime(byte matchTime);
    }
}