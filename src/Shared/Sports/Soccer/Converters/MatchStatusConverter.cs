namespace LiveScore.Soccer.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Common.Services;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Models.Matches;

    public class MatchStatusConverter : IMatchStatusConverter
    {
        private static readonly IDictionary<MatchStatus, string> StatusMapper = new Dictionary<MatchStatus, string>
        {
            { MatchStatus.Postponed, AppResources.Postp },
            { MatchStatus.StartDelayed, AppResources.StartDelayed },
            { MatchStatus.Cancelled, AppResources.Canc },
            { MatchStatus.AwaitingPenalties, AppResources.AwaitPen },
            { MatchStatus.Penalties, AppResources.Pen },
            { MatchStatus.Pause, AppResources.Pause },
            { MatchStatus.Interrupted, AppResources.INT },
            { MatchStatus.Halftime, AppResources.HT },
            { MatchStatus.Delayed, AppResources.Delayed },
            { MatchStatus.Abandoned, AppResources.AB },
            { MatchStatus.FullTime, AppResources.FT },
            { MatchStatus.Ended, AppResources.FT },
            { MatchStatus.Closed, AppResources.FT },
            { MatchStatus.EndedAfterPenalties, AppResources.AP },
            { MatchStatus.EndedExtraTime, AppResources.AET },
            { MatchStatus.AwaitingExtraTime, AppResources.AwaitET },
            { MatchStatus.ExtraTimeHalfTime, AppResources.ETHT }
        };

        private static readonly IDictionary<MatchStatus, int> PeriodEndTimes = new Dictionary<MatchStatus, int>
        {
            { MatchStatus.FirstHalf, 45 },
            { MatchStatus.SecondHalf, 90 },
            { MatchStatus.FirstHalfExtra, 105 },
            { MatchStatus.SecondHalfExtra, 120 }
        };

        private static readonly DateTime InjuryTimeCacheExpiration = DateTime.Now.AddMinutes(15);
        private readonly ICachingService cacheService;

        public MatchStatusConverter(ICachingService localStorage)
        {
            this.cacheService = localStorage;
        }

        public string BuildStatus(IMatchSummary match)
        {
            if (match == null)
            {
                return AppResources.FT;
            }

            var soccerMatch = match as MatchSummary;

            if (soccerMatch.EventStatus == null)
            {
                return AppResources.FT;
            }

            if (soccerMatch.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToLocalTimeWithoutSecond();
            }

            if (soccerMatch.EventStatus.IsLive)
            {
                return BuildStatusForLive(soccerMatch);
            }

            if (soccerMatch.EventStatus.IsClosed)
            {
                var status = BuildMatchStatus(soccerMatch);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildEventStatus(soccerMatch);
        }

        private string BuildStatusForLive(MatchSummary match)
        {
            var status = BuildMatchStatus(match);

            if (!string.IsNullOrEmpty(status))
            {
                return status;
            }

            var stoppageTimeHasValue = !string.IsNullOrEmpty(match.StoppageTime) && match.StoppageTime != "0";

            if (match.LastTimelineType != null && (match.LastTimelineType == EventType.InjuryTimeShown || stoppageTimeHasValue))
            {
                return BuildMatchInjuryTime(match);
            }

            return match.MatchTime + "'";
        }

        private static string BuildMatchStatus(MatchSummary match)
        {
            var matchStatus = match.MatchStatus;

            if (matchStatus?.Value != null && StatusMapper.ContainsKey(matchStatus))
            {
                return StatusMapper[matchStatus];
            }

            return string.Empty;
        }

        private static string BuildEventStatus(MatchSummary match)

        {
            var eventStatus = match.EventStatus;

            if (eventStatus?.Value != null && StatusMapper.ContainsKey(eventStatus))
            {
                return StatusMapper[eventStatus];
            }

            return string.Empty;
        }

        private string BuildMatchInjuryTime(MatchSummary match)
        {
            PeriodEndTimes.TryGetValue(match.MatchStatus, out int periodEndTime);
            var cacheKey = "InjuryTimeAnnouced" + match.Id;
            var annoucedInjuryTime = Task.Run(() => cacheService.GetValueOrDefaultInMemory(cacheKey, 0)).Result;

            if (match.InjuryTimeAnnounced > 0)
            {
                Task.Run(() => cacheService.InsertValueInMemory(cacheKey, match.InjuryTimeAnnounced, InjuryTimeCacheExpiration)).Wait();
                annoucedInjuryTime = match.InjuryTimeAnnounced;
            }

            var currentInjuryTime = match.MatchTime - periodEndTime;
            var displayInjuryTime = currentInjuryTime == 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime < 0 || currentInjuryTime > annoucedInjuryTime)
            {
                displayInjuryTime = annoucedInjuryTime;
            }

            return $"{periodEndTime}+{displayInjuryTime}'";
        }
    }
}