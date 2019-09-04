namespace LiveScore.Soccer.Models.Matches
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
            DateTimeOffset currentPeriodStartTime,
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
            byte aggregateHomeScore,
            byte aggregateAwayScore,
            byte homeRedCards,
            byte homeYellowRedCards,
            byte awayRedCards,
            byte awayYellowRedCards,
            byte matchTime,
            string stoppageTime,
            byte injuryTimeAnnounced,
            EventType lastTimelineType,
            IEnumerable<MatchPeriod> matchPeriods)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Id = id;
            EventDate = eventDate;
            CurrentPeriodStartTime = currentPeriodStartTime;
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
            AggregateHomeScore = aggregateHomeScore;
            AggregateAwayScore = aggregateAwayScore;
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

        /// <summary>
        /// Keep private setter for Json Serializer
        /// </summary>
        public string Id { get; private set; }

        public DateTimeOffset EventDate { get; private set; }

        public DateTimeOffset CurrentPeriodStartTime { get; private set; }

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

        public byte AggregateHomeScore { get; private set; }

        public byte AggregateAwayScore { get; private set; }

        public byte HomeRedCards { get; private set; }

        public byte HomeYellowRedCards { get; private set; }

        public byte AwayRedCards { get; private set; }

        public byte AwayYellowRedCards { get; private set; }

        public byte MatchTime { get; private set; }

        public string StoppageTime { get; private set; }

        public byte InjuryTimeAnnounced { get; private set; }

        public EventType LastTimelineType { get; private set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; private set; }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public void UpdateCurrentPeriodStartTime(DateTimeOffset currentPeriodStartTime)
        {
            CurrentPeriodStartTime = currentPeriodStartTime;
        }

        public void UpdateResult(IMatchResult matchResult)
        {
            if (!(matchResult is MatchResult soccerMatchResult))
            {
                return;
            }

            EventStatus = soccerMatchResult.EventStatus;
            MatchStatus = soccerMatchResult.MatchStatus;
            MatchTime = soccerMatchResult.MatchTime;
            MatchPeriods = soccerMatchResult.MatchPeriods;
            HomeScore = soccerMatchResult.HomeScore;
            AwayScore = soccerMatchResult.AwayScore;
            WinnerId = soccerMatchResult.WinnerId;
            AggregateWinnerId = soccerMatchResult.WinnerId;
        }

        public void UpdateLastTimeline(ITimelineEvent timelineEvent)
        {
            if (!(timelineEvent is TimelineEvent soccerTimeline))
            {
                return;
            }

            LastTimelineType = soccerTimeline.Type;
            StoppageTime = soccerTimeline.StoppageTime;
            InjuryTimeAnnounced = soccerTimeline.InjuryTimeAnnounced;
        }

        public void UpdateTeamStatistic(ITeamStatistic teamStatistic, bool isHome)
        {
            if (!(teamStatistic is TeamStatistic soccerTeamStats))
            {
                return;
            }

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

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        public string HomePenaltyImage
            => HomeWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        public string AwayPenaltyImage
             => AwayWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        public string HomeSecondLegImage
              => HomeWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        public string AwaySecondLegImage
               => AwayWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        public bool IsInExtraTime
            => EventStatus?.IsLive == true
            && MatchStatus?.IsInExtraTime == true;

        public bool IsInLiveAndNotExtraTime
            => EventStatus != null && EventStatus.IsLive
            && MatchStatus?.IsInExtraTime == false;

        public byte TotalHomeRedCards => (byte)(HomeRedCards + HomeYellowRedCards);

        public byte TotalAwayRedCards => (byte)(AwayRedCards + AwayYellowRedCards);

        public MatchPeriod GetPenaltyResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);

        public MatchPeriod GetOvertimeResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsOvertime == true);

        public bool HasFullTimeResult()
            => MatchPeriods?.Count() >= NumberOfFullTimePeriodsResult;

        private bool HomeWinPenalty
           => EventStatus?.IsClosed == true && GetPenaltyResult() != null && HomeTeamId == WinnerId;

        private bool AwayWinPenalty
            => EventStatus?.IsClosed == true && GetPenaltyResult() != null && AwayTeamId == WinnerId;

        private bool HomeWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && HomeTeamId == AggregateWinnerId);

        private bool AwayWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && AwayTeamId == AggregateWinnerId);
    }
}