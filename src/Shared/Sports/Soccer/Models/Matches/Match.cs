namespace LiveScore.Soccer.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using MessagePack;
    using PropertyChanged;
    using Teams;

    [AddINotifyPropertyChangedInterface, MessagePackObject]
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
            IEnumerable<MatchPeriod> matchPeriods,
            string countryCode,
            string countryName,
            DateTimeOffset modifiedTime,
            bool isInternationalLeague)
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
            CountryCode = countryCode;
            CountryName = countryName;
            ModifiedTime = modifiedTime;
            IsInternationalLeague = isInternationalLeague;
        }

        internal Match(DateTime eventDate, IMatchResult matchResult) : this(matchResult)
        {
            EventDate = eventDate;
        }

        internal Match(IMatchResult matchResult)
        {
            UpdateResult(matchResult);
        }

        [Key(0)]
        public string Id { get; private set; }

        [Key(1)]
        public DateTimeOffset EventDate { get; private set; }

        [Key(2)]
        public DateTimeOffset CurrentPeriodStartTime { get; private set; }

        [Key(3)]
        public string LeagueId { get; private set; }

        [Key(4)]
        public string LeagueName { get; private set; }

        [Key(5)]
        public string HomeTeamId { get; private set; }

        [Key(6)]
        public string HomeTeamName { get; private set; }

        [Key(7)]
        public string AwayTeamId { get; private set; }

        [Key(8)]
        public string AwayTeamName { get; private set; }

        [Key(9)]
        public MatchStatus MatchStatus { get; private set; }

        [Key(10)]
        public MatchStatus EventStatus { get; private set; }

        [Key(11)]
        public byte HomeScore { get; private set; }

        [Key(12)]
        public byte AwayScore { get; private set; }

        [Key(13)]
        public string WinnerId { get; private set; }

        [Key(14)]
        public string AggregateWinnerId { get; private set; }

        [Key(15)]
        public byte AggregateHomeScore { get; private set; }

        [Key(16)]
        public byte AggregateAwayScore { get; private set; }

        [Key(17)]
        public byte HomeRedCards { get; private set; }

        [Key(18)]
        public byte HomeYellowRedCards { get; private set; }

        [Key(19)]
        public byte AwayRedCards { get; private set; }

        [Key(20)]
        public byte AwayYellowRedCards { get; private set; }

        [Key(21)]
        public byte MatchTime { get; private set; }

        [Key(22)]
        public string StoppageTime { get; private set; }

        [Key(23)]
        public byte InjuryTimeAnnounced { get; private set; }

        [Key(24)]
        public EventType LastTimelineType { get; private set; }

        [Key(25)]
        public IEnumerable<MatchPeriod> MatchPeriods { get; private set; }

        [Key(26)]
        public string CountryCode { get; private set; }

        [Key(27)]
        public string CountryName { get; private set; }

        [Key(28)]
        public DateTimeOffset ModifiedTime { get; private set; }

        [Key(29)]
        public bool IsInternationalLeague { get; private set; }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public void UpdateMatch(IMatch match)
        {
            if (!(match is Match soccerMatch))
            {
                return;
            }

            EventStatus = soccerMatch.EventStatus;
            MatchStatus = soccerMatch.MatchStatus;
            MatchTime = soccerMatch.MatchTime;
            MatchPeriods = soccerMatch.MatchPeriods;
            HomeScore = soccerMatch.HomeScore;
            AwayScore = soccerMatch.AwayScore;
            WinnerId = soccerMatch.WinnerId;
            AggregateWinnerId = soccerMatch.AggregateWinnerId;
            AggregateHomeScore = soccerMatch.AggregateHomeScore;
            AggregateAwayScore = soccerMatch.AggregateAwayScore;
            LastTimelineType = soccerMatch.LastTimelineType;
            StoppageTime = soccerMatch.StoppageTime;
            InjuryTimeAnnounced = soccerMatch.InjuryTimeAnnounced;
            HomeRedCards = soccerMatch.HomeRedCards;
            HomeYellowRedCards = soccerMatch.HomeYellowRedCards;
            AwayRedCards = soccerMatch.AwayRedCards;
            AwayYellowRedCards = soccerMatch.AwayYellowRedCards;
        }

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
            AggregateWinnerId = soccerMatchResult.AggregateWinnerId;
            AggregateHomeScore = soccerMatchResult.AggregateHomeScore;
            AggregateAwayScore = soccerMatchResult.AggregateAwayScore;
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

        [IgnoreMember]
        public string HomePenaltyImage
            => HomeWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        [IgnoreMember]
        public string AwayPenaltyImage
             => AwayWinPenalty ? Enumerations.Images.PenaltyWinner.Value : string.Empty;

        [IgnoreMember]
        public string HomeSecondLegImage
              => HomeWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        [IgnoreMember]
        public string AwaySecondLegImage
               => AwayWinSecondLeg ? Enumerations.Images.SecondLeg.Value : string.Empty;

        [IgnoreMember]
        public bool IsInExtraTime
            => EventStatus?.IsLive == true
            && MatchStatus?.IsInExtraTime == true;

        [IgnoreMember]
        public bool IsInLiveAndNotExtraTime
            => EventStatus != null && EventStatus.IsLive
            && MatchStatus?.IsInExtraTime == false;

        [IgnoreMember]
        public byte TotalHomeRedCards => (byte)(HomeRedCards + HomeYellowRedCards);

        [IgnoreMember]
        public byte TotalAwayRedCards => (byte)(AwayRedCards + AwayYellowRedCards);

        public MatchPeriod GetPenaltyResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);

        public MatchPeriod GetOvertimeResult()
            => MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsOvertime == true);

        public bool HasFullTimeResult()
            => MatchPeriods?.Count() >= NumberOfFullTimePeriodsResult;

        [IgnoreMember]
        public string LeagueGroupName
            => $"{(IsInternationalLeague ? string.Empty : CountryName + " ")}{LeagueName}".ToUpperInvariant();

        [IgnoreMember]
        private bool HomeWinPenalty
           => EventStatus?.IsClosed == true && GetPenaltyResult() != null && HomeTeamId == WinnerId;

        [IgnoreMember]
        private bool AwayWinPenalty
            => EventStatus?.IsClosed == true && GetPenaltyResult() != null && AwayTeamId == WinnerId;

        [IgnoreMember]
        private bool HomeWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && HomeTeamId == AggregateWinnerId);

        [IgnoreMember]
        private bool AwayWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && AwayTeamId == AggregateWinnerId);

        public override bool Equals(object obj)
            => (obj is Match actualObj) && Id == actualObj.Id;

        public override int GetHashCode() => Id?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject]
    public class MatchList
    {
        [Key(0)]
        public IEnumerable<Match> Matches { get; set; }
    }
}