using System;
using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;

namespace LiveScore.Core.Models.Matches
{
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

        byte HomeScore { get; }

        byte AwayScore { get; }

        string HomeTeamId { get; }

        string HomeTeamName { get; }

        string AwayTeamId { get; }

        string AwayTeamName { get; }

        string CountryCode { get; }

        string LeagueGroupName { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        string HomePenaltyImage { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        string AwayPenaltyImage { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        string HomeSecondLegImage { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        string AwaySecondLegImage { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        byte TotalHomeRedCards { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        byte TotalAwayRedCards { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        bool IsInExtraTime { get; }

        /// <summary>
        /// Temporary add Soccer properties for compiled binding
        /// </summary>
        bool IsInLiveAndNotExtraTime { get; }

        string LeagueSeasonId { get; }

        string WinnerId { get; }

        DateTimeOffset ModifiedTime { get; }

        bool IsInternationalLeague { get; }

        int LeagueOrder { get; }

        void UpdateMatch(IMatch match);

        void UpdateResult(IMatchResult matchResult);

        void UpdateLastTimeline(ITimelineEvent timelineEvent);

        void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome);
    }
}