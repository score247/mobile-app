using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using MessagePack;
using PropertyChanged;

[assembly: InternalsVisibleToAttribute("Soccer.Tests")]

namespace LiveScore.Soccer.Models.Matches
{
    [AddINotifyPropertyChangedInterface, MessagePackObject(keyAsPropertyName: true)]
    public class SoccerMatch : IMatch
    {
        private const int NumberOfFullTimePeriodsResult = 2;
        private DateTimeOffset currentPeriodStartTime;

#pragma warning disable S107 // Methods should not have too many parameters

        [SerializationConstructor]
        public SoccerMatch(
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
            bool isInternationalLeague,
            int leagueOrder,
            string leagueSeasonId,
            LeagueRoundType leagueRoundType,
            string leagueRoundName,
            int leagueRoundNumber,
            string leagueRoundGroup,
            string leagueGroupName,
            Coverage coverage,
            bool leagueHasStandings,
            string groupName)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Id = id;
            EventDate = eventDate;
            this.currentPeriodStartTime = currentPeriodStartTime == DateTimeOffset.MinValue ? eventDate : currentPeriodStartTime;
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
            LeagueOrder = leagueOrder;
            LeagueSeasonId = leagueSeasonId;
            LeagueRoundType = leagueRoundType;
            LeagueRoundName = leagueRoundName;
            LeagueRoundNumber = leagueRoundNumber;
            LeagueRoundGroup = leagueRoundGroup;
            LeagueGroupName = string.IsNullOrWhiteSpace(leagueGroupName) ? leagueName : leagueGroupName;
            Coverage = coverage;
            LeagueHasStandings = leagueHasStandings;
            GroupName = groupName;
        }

        internal SoccerMatch(DateTime eventDate, IMatchResult matchResult) : this(matchResult)
        {
            EventDate = eventDate;
        }

        internal SoccerMatch(IMatchResult matchResult)
        {
            UpdateResult(matchResult);
        }

        public string Id { get; private set; }

        public DateTimeOffset EventDate { get; private set; }

        public DateTimeOffset CurrentPeriodStartTime
        {
            get => currentPeriodStartTime == DateTimeOffset.MinValue
                ? EventDate
                : currentPeriodStartTime;
            set => currentPeriodStartTime = value;
        }

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

        public byte InjuryTimeAnnounced { get; set; }

        public EventType LastTimelineType { get; private set; }

        public IEnumerable<MatchPeriod> MatchPeriods { get; private set; }

        public string CountryCode { get; private set; }

        public string CountryName { get; private set; }

        public DateTimeOffset ModifiedTime { get; private set; }

        public bool IsInternationalLeague { get; private set; }

        public int LeagueOrder { get; private set; }

        public string LeagueSeasonId { get; private set; }

        public LeagueRoundType LeagueRoundType { get; private set; }

        public string LeagueRoundName { get; private set; }

        public int LeagueRoundNumber { get; private set; }

        public string LeagueRoundGroup { get; private set; }

        public string LeagueGroupName { get; private set; }

        public Coverage Coverage { get; private set; }

        public bool LeagueHasStandings { get; private set; }

        public string GroupName { get; private set; }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public void UpdateMatch(IMatch match)
        {
            if (match == null)
            {
                return;
            }

            if (!(match is SoccerMatch soccerMatch))
            {
                return;
            }

            EventDate = soccerMatch.EventDate;
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
            CurrentPeriodStartTime = soccerMatch.CurrentPeriodStartTime;
            ModifiedTime = soccerMatch.ModifiedTime;
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

            if (soccerTimeline.Type.IsInjuryTimeShown)
            {
                InjuryTimeAnnounced = soccerTimeline.InjuryTimeAnnounced;
            }
        }

        public void UpdateTeamStatistic(Core.Models.Teams.ITeamStatistic teamStatistic, bool isHome)
        {
            if (!(teamStatistic is global::LiveScore.Soccer.Models.Teams.TeamStatistic soccerTeamStats))
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
            => EventStatus?.IsLive == true && MatchStatus?.IsInExtraTime == false;

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
        private bool HomeWinPenalty
           => EventStatus?.IsClosed == true && GetPenaltyResult() != null && HomeTeamId == (AggregateWinnerId ?? WinnerId);

        [IgnoreMember]
        private bool AwayWinPenalty
            => EventStatus?.IsClosed == true && GetPenaltyResult() != null && AwayTeamId == (AggregateWinnerId ?? WinnerId);

        [IgnoreMember]
        private bool HomeWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && HomeTeamId == AggregateWinnerId);

        [IgnoreMember]
        private bool AwayWinSecondLeg
          => EventStatus?.IsClosed == true && (!string.IsNullOrEmpty(AggregateWinnerId) && AwayTeamId == AggregateWinnerId);

        public override bool Equals(object obj)
            => (obj is SoccerMatch actualObj) && Id == actualObj.Id;

        public override int GetHashCode() => Id?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchList
    {
        public IEnumerable<SoccerMatch> Matches { get; set; }
    }
}