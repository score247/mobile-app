namespace LiveScore.Soccer.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    // TODO: Unit test will be written in Performance Enhancement branch
    public class MatchMinuteConverter : IMatchMinuteConverter
    {
        private static readonly IDictionary<MatchStatus, int> PeriodStartMinute = new Dictionary<MatchStatus, int>
        {
            { MatchStatus.FirstHalf, 1 },
            { MatchStatus.SecondHalf, 46 },
            { MatchStatus.FirstHalfExtra, 91 },
            { MatchStatus.SecondHalfExtra, 106 }
        };

        private static readonly IDictionary<MatchStatus, int> PeriodEndMinute = new Dictionary<MatchStatus, int>
        {
            { MatchStatus.FirstHalf, 45 },
            { MatchStatus.SecondHalf, 90 },
            { MatchStatus.FirstHalfExtra, 105 },
            { MatchStatus.SecondHalfExtra, 120 }
        };

        private static readonly DateTime InjuryTimeCacheExpiration = DateTime.Now.AddMinutes(15);

        private readonly ICachingService cachingService;
        private IMatch match;

        public MatchMinuteConverter(ICachingService cachingService)
        {
            this.cachingService = cachingService;
        }

        public string BuildMatchMinute(IMatch match)
        {
            this.match = match;
            PeriodStartMinute.TryGetValue(match.MatchResult?.MatchStatus, out int periodStartMinute);
            PeriodEndMinute.TryGetValue(match.MatchResult?.MatchStatus, out int periodEndMinute);

            var periodStartTime = this.match.CurrentPeriodStartTime == DateTimeOffset.MinValue
                    ? this.match.EventDate
                    : this.match.CurrentPeriodStartTime;

            // TODO: What if CurrentPeriodStartTime does not have data?
            var matchMinute = (int)(periodStartMinute + (DateTime.Now - periodStartTime).TotalMinutes);
            var lastTimeline = match.LatestTimeline;

            if (lastTimeline?.Type == EventType.InjuryTimeShown || GetAnnouncedInjuryTime() > 0)
            {
                return BuildMinuteWithInjuryTime(lastTimeline, matchMinute, periodEndMinute);
            }

            if (matchMinute >= periodEndMinute)
            {
                matchMinute = periodEndMinute;
            }

            if (matchMinute <= 0)
            {
                matchMinute = periodStartMinute;
            }

            return $"{matchMinute}'";
        }

        private string BuildMinuteWithInjuryTime(ITimelineEvent timeline, int matchMinute, int periodEndMinute)
        {
            var annoucedInjuryTime = GetAnnouncedInjuryTime();

            if (timeline.InjuryTimeAnnounced > 0)
            {
                UpdateAnnouncedInjuryTime(timeline.InjuryTimeAnnounced);
                annoucedInjuryTime = timeline.InjuryTimeAnnounced;
            }

            var currentInjuryTime = matchMinute - periodEndMinute;
            var displayInjuryTime = currentInjuryTime <= 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime > annoucedInjuryTime)
            {
                displayInjuryTime = annoucedInjuryTime;
            }

            return $"{periodEndMinute}+{displayInjuryTime}'";
        }

        private int GetAnnouncedInjuryTime()
        {
            var cacheKey = $"InjuryTimeAnnouced_{match.Id}_{match.MatchResult.MatchStatus.DisplayName}";
            var annoucedInjuryTime = Task.Run(() => cachingService.GetValueOrDefaultInMemory(cacheKey, 0)).Result;

            return annoucedInjuryTime;
        }

        public void UpdateAnnouncedInjuryTime(int injuryTime)
        {
            var cacheKey = $"InjuryTimeAnnouced_{match.Id}_{match.MatchResult.MatchStatus.DisplayName}";

            Task.Run(() => cachingService.InsertValueInMemory(cacheKey, injuryTime, InjuryTimeCacheExpiration)).Wait();
        }
    }
}