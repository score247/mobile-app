namespace LiveScore.Soccer.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Common.Services;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public class MatchStatusConverter : IMatchStatusConverter
    {
        private static readonly IDictionary<string, string> StatusMapper = new Dictionary<string, string>
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

        private static readonly IDictionary<string, string> FullStatusMapper = new Dictionary<string, string>
        {
            { MatchStatus.Postponed, AppResources.Postponed },
            { MatchStatus.StartDelayed, AppResources.StartDelayed },
            { MatchStatus.Cancelled, AppResources.Cancelled },
            { MatchStatus.AwaitingPenalties, AppResources.AwaitPenalties },
            { MatchStatus.Penalties, AppResources.Penalties },
            { MatchStatus.Pause, AppResources.Pause },
            { MatchStatus.Interrupted, AppResources.Interrupted },
            { MatchStatus.Halftime, AppResources.HalfTime },
            { MatchStatus.Delayed, AppResources.Delayed },
            { MatchStatus.Abandoned, AppResources.Abandoned },
            { MatchStatus.FullTime, AppResources.FullTime },
            { MatchStatus.Ended, AppResources.FullTime },
            { MatchStatus.Closed, AppResources.FullTime },
            { MatchStatus.EndedAfterPenalties, AppResources.AfterPenalties },
            { MatchStatus.EndedExtraTime, AppResources.AfterExtraTime },
            { MatchStatus.AwaitingExtraTime, AppResources.AwaitExtraTime },
            { MatchStatus.ExtraTimeHalfTime, AppResources.ExtraTimeHalfTime }
        };

        private static readonly IDictionary<string, int> PeriodEndTimes = new Dictionary<string, int>
        {
            { MatchStatus.FirstHaft, 45 },
            { MatchStatus.SecondHaft, 90 },
            { MatchStatus.FirstHaftExtra, 105 },
            { MatchStatus.SecondHaftExtra, 120 }
        };

        private static readonly DateTime InjuryTimeCacheExpiration = DateTime.Now.AddMinutes(15);
        private readonly ILocalStorage localStorage;

        public MatchStatusConverter(ILocalStorage localStorage)
        {
            this.localStorage = localStorage;
        }

        public string BuildStatus(IMatch match, bool showFullStatus = false)
        {
            if (match == null)
            {
                return AppResources.FT;
            }

            if (match.MatchResult == null || match.MatchResult.EventStatus == null)
            {
                return AppResources.FT;
            }

            if (match.MatchResult.EventStatus.IsNotStarted)
            {
                return match.EventDate.ToTimeWithoutSecond();
            }

            if (match.MatchResult.EventStatus.IsLive)
            {
                return BuildStatusForLive(match, showFullStatus);
            }

            if (match.MatchResult.EventStatus.IsClosed)
            {
                var status = BuildMatchStatus(match, showFullStatus);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildEventStatus(match, showFullStatus);
        }

        private string BuildStatusForLive(IMatch match, bool showFullStatus)
        {
            var status = BuildMatchStatus(match, showFullStatus);

            if (!string.IsNullOrEmpty(status))
            {
                return status;
            }

            var timeline = match.LatestTimeline;
            var stoppageTimeHasValue = !string.IsNullOrEmpty(timeline?.StoppageTime) && timeline?.StoppageTime != "0";

            if (timeline != null && (timeline.Type == EventTypes.InjuryTimeShown || stoppageTimeHasValue))
            {
                return BuildMatchInjuryTime(match, timeline);
            }

            return match.MatchResult.MatchTime + "'";
        }

        private static string BuildMatchStatus(IMatch match, bool showFullStatus)
        {
            var statusMapper = showFullStatus ? FullStatusMapper : StatusMapper;
            var matchStatus = match.MatchResult?.MatchStatus;

            if (matchStatus?.Value != null && statusMapper.ContainsKey(matchStatus.Value))
            {
                return statusMapper[matchStatus.Value];
            }

            return string.Empty;
        }

        private static string BuildEventStatus(IMatch match, bool showFullStatus)

        {
            var statusMapper = showFullStatus ? FullStatusMapper : StatusMapper;
            var eventStatus = match.MatchResult?.EventStatus;

            if (eventStatus?.Value != null && statusMapper.ContainsKey(eventStatus.Value))
            {
                return statusMapper[eventStatus.Value];
            }

            return string.Empty;
        }

        private string BuildMatchInjuryTime(IMatch match, ITimeline timeline)
        {
            PeriodEndTimes.TryGetValue(match.MatchResult.MatchStatus.Value, out int periodEndTime);
            var cacheKey = "InjuryTimeAnnouced" + match.Id;
            var annoucedInjuryTime = Task.Run(() => localStorage.GetValueOrDefault(cacheKey, 0)).Result;

            if (timeline.InjuryTimeAnnounced > 0)
            {
                Task.Run(() => localStorage.InsertValue(cacheKey, timeline.InjuryTimeAnnounced, InjuryTimeCacheExpiration)).Wait();
                annoucedInjuryTime = timeline.InjuryTimeAnnounced;
            }

            var currentInjuryTime = match.MatchResult.MatchTime - periodEndTime;
            var displayInjuryTime = currentInjuryTime == 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime < 0 || currentInjuryTime > annoucedInjuryTime)
            {
                displayInjuryTime = annoucedInjuryTime;
            }

            return $"{periodEndTime}+{displayInjuryTime}'";
        }
    }
}