using System;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;

namespace LiveScore.Core.Models.Matches
{
    public interface IMatch
    {
        string Id { get; }

        DateTimeOffset EventDate { get; }

        string LeagueId { get; }

        string LeagueName { get; }

        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }

        void UpdateResult(IMatchResult matchResult);

        void UpdateLastTimeline(ITimelineEvent timelineEvent);

        void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome);
    }
}