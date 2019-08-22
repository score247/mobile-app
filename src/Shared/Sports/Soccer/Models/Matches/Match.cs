﻿namespace LiveScore.Soccer.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Teams;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class Match : IMatch
    {
        private const int NumberOfFullTimePeriodsResult = 2;

#pragma warning disable S107 // Methods should not have too many parameters

        public Match(
            string id,
            DateTimeOffset eventDate,
            string leagueId,
            string leagueName,
            string homeTeamId,
            string homeTeamName,
            string awayTeamId,
            string awayTeamName,
            MatchStatus matchStatus,
            MatchStatus eventStatus,
            byte homeScore,
            byte awayScore,
            string winnerId,
            string aggregateWinnerId,
            byte homeRedCards,
            byte homeYellowRedCards,
            byte awayRedCards,
            byte awayYellowRedCards,
            byte matchTime,
            string stoppageTime,
            byte injuryTimeAnnounced,
            EventType lastTimelineType,
            IEnumerable<MatchPeriod> matchPeriods)
        {
            Id = id;
            EventDate = eventDate;
            LeagueId = leagueId;
            LeagueName = leagueName;
            HomeTeamId = homeTeamId;
            HomeTeamName = homeTeamName;
            AwayTeamId = awayTeamId;
            AwayTeamName = awayTeamName;
            MatchStatus = matchStatus;
            EventStatus = eventStatus;
            HomeScore = homeScore;
            AwayScore = awayScore;
            WinnerId = winnerId;
            AggregateWinnerId = aggregateWinnerId;
            HomeRedCards = homeRedCards;
            HomeYellowRedCards = homeYellowRedCards;
            AwayRedCards = awayRedCards;
            AwayYellowRedCards = awayYellowRedCards;
            MatchTime = matchTime;
            StoppageTime = stoppageTime;
            InjuryTimeAnnounced = injuryTimeAnnounced;
            LastTimelineType = lastTimelineType;
            MatchPeriods = matchPeriods;
        }

#pragma warning restore S107 // Methods should not have too many parameters

        /// <summary>
        /// Keep private setter for Json Serializer
        /// </summary>
        public string Id { get; private set; }

        public DateTimeOffset EventDate { get; private set; }

        public string LeagueId { get; private set; }

        public string LeagueName { get; private set; }

        public string HomeTeamId { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamId { get; private set; }

        public string AwayTeamName { get; private set; }

        public MatchStatus MatchStatus { get; private set; }

        public MatchStatus EventStatus { get; private set; }

        public byte HomeScore { get; private set; }

        public byte AwayScore { get; private set; }

        public string WinnerId { get; private set; }

        public string AggregateWinnerId { get; private set; }

        public byte HomeRedCards { get; private set; }

        public byte HomeYellowRedCards { get; private set; }

        public byte AwayRedCards { get; private set; }

        public byte AwayYellowRedCards { get; private set; }

        public byte MatchTime { get; private set; }

        public string StoppageTime { get; private set; }

        public byte InjuryTimeAnnounced { get; private set; }

        public EventType LastTimelineType { get; private set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; private set; }

        public void UpdateResult(IMatchResult matchResult)
        {
            EventStatus = matchResult.EventStatus;
            MatchStatus = matchResult.MatchStatus;
            MatchTime = matchResult.MatchTime;
            MatchPeriods = matchResult.MatchPeriods;
            HomeScore = matchResult.HomeScore;
            AwayScore = matchResult.AwayScore;
            WinnerId = matchResult.WinnerId;
            AggregateWinnerId = matchResult.WinnerId;
        }

        public void UpdateLastTimeline(ITimelineEvent timelineEvent)
        {
            LastTimelineType = timelineEvent.Type;
            StoppageTime = timelineEvent.StoppageTime;
            InjuryTimeAnnounced = timelineEvent.InjuryTimeAnnounced;
        }

        public void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome)
        {
            var soccerTeamStats = teamStatistic as TeamStatistic;

            if (isHome)
            {
                HomeRedCards = soccerTeamStats.RedCards;
                HomeYellowRedCards = soccerTeamStats.YellowRedCards;
            }
            else
            {
                AwayRedCards = soccerTeamStats.RedCards;
                AwayYellowRedCards = soccerTeamStats.YellowRedCards;
            }
        }

        public string HomePenaltyImage
            => HomeWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        public string AwayPenaltyImage
             => AwayWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        public string HomeSecondLegImage
              => HomeWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        public string AwaySecondLegImage
               => AwayWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        public bool IsInExtraTime
            => EventStatus != null && EventStatus.IsLive
            && MatchStatus != null && MatchStatus.IsInExtraTime;

        public bool IsInLiveAndNotExtraTime
            => EventStatus != null && EventStatus.IsLive
            && MatchStatus != null && !MatchStatus.IsInExtraTime;

        public byte TotalHomeRedCards => (byte)(HomeRedCards + HomeYellowRedCards);

        public byte TotalAwayRedCards => (byte)(AwayRedCards + AwayYellowRedCards);

        public MatchPeriod GetPenaltyResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);

        public MatchPeriod GetOvertimeResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsOvertime == true);

        public bool HasFullTimeResult()
            => MatchPeriods?.Count() >= NumberOfFullTimePeriodsResult;

        private bool HomeWinPenalty
           => EventStatus != null && EventStatus.IsClosed && GetPenaltyResult() != null && HomeTeamId == WinnerId;

        private bool AwayWinPenalty
            => EventStatus != null && EventStatus.IsClosed && GetPenaltyResult() != null && AwayTeamId == WinnerId;

        private bool HomeWinSecondLeg
          => EventStatus != null && EventStatus.IsClosed && (!string.IsNullOrEmpty(AggregateWinnerId) && HomeTeamId == AggregateWinnerId);

        private bool AwayWinSecondLeg
          => EventStatus != null && EventStatus.IsClosed && (!string.IsNullOrEmpty(AggregateWinnerId) && AwayTeamId == AggregateWinnerId);
    }
}