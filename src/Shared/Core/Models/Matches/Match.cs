namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Teams;

    public interface IMatch
    {
        string Id { get; }

        DateTimeOffset EventDate { get; }
        DateTimeOffset CurrentPeriodStartTime { get; set; }


        string LeagueId { get; }

        string LeagueName { get; }

        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        IEnumerable<MatchPeriod> MatchPeriods { get; }

        void UpdateResult(IMatchResult matchResult);

        void UpdateLastTimeline(ITimelineEvent timelineEvent);

        void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome);
    }
}