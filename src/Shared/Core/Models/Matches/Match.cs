namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using Enumerations;
    using Teams;

    public interface IMatch
    {
        string Id { get; }

        DateTimeOffset EventDate { get; }

        DateTimeOffset CurrentPeriodStartTime { get; }

        string LeagueId { get; }

        string LeagueName { get; }

        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        string CountryCode { get; }

        string LeagueGroupName { get; }

        void UpdateCurrentPeriodStartTime(DateTimeOffset currentPeriodStartTime);

        void UpdateResult(IMatchResult matchResult);

        void UpdateLastTimeline(ITimelineEvent timelineEvent);

        void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome);
    }
}