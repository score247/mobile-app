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
        private readonly ILocalStorage localStorage;

        public MatchStatusConverter(ILocalStorage localStorage)
        {
            this.localStorage = localStorage;
        }

        public string BuildStatus(IMatch match)
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

            if (match.MatchResult.EventStatus.IsLive())
            {
                return BuildStatusForLive(match);
            }

            if (match.MatchResult.EventStatus.IsClosed())
            {
                var status = BuildMatchStatus(match);

                return string.IsNullOrEmpty(status) ? AppResources.FT : status;
            }

            return BuildEventStatus(match);
        }

        private string BuildStatusForLive(IMatch match)
        {
            var status = BuildMatchStatus(match);

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

        private static string BuildMatchStatus(IMatch match)
        {
            var matchStatus = match.MatchResult?.MatchStatus;

            if (matchStatus?.Value != null && StatusMapper.ContainsKey(matchStatus))
            {
                return StatusMapper[matchStatus];
            }

            return string.Empty;
        }

        private static string BuildEventStatus(IMatch match)

        {
            var eventStatus = match.MatchResult?.EventStatus;

            if (eventStatus?.Value != null && StatusMapper.ContainsKey(eventStatus))
            {
                return StatusMapper[eventStatus];
            }

            return string.Empty;
        }

        private string BuildMatchInjuryTime(IMatch match, ITimeline timeline)
        {
            PeriodEndTimes.TryGetValue(match.MatchResult.MatchStatus, out int periodEndTime);
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