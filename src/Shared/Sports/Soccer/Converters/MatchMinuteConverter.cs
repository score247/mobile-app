namespace LiveScore.Soccer.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Models.Matches;

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

        private readonly ICachingService cachingService;
        private Match soccerMatch;

        public MatchMinuteConverter(ICachingService cachingService)
        {
            this.cachingService = cachingService;
        }

        public string BuildMatchMinute(IMatch match)
        {
            soccerMatch = match as Match;


            if (soccerMatch == null)
            {
                return string.Empty;
            }

            PeriodStartMinute.TryGetValue(match.MatchStatus, out var periodStartMinute);
            PeriodEndMinute.TryGetValue(match.MatchStatus, out var periodEndMinute);

            var periodStartTime = soccerMatch != null && soccerMatch.CurrentPeriodStartTime == DateTimeOffset.MinValue
                ? soccerMatch.EventDate
                : soccerMatch.CurrentPeriodStartTime;

            // TODO: What if CurrentPeriodStartTime does not have data?
            var matchMinute = (int)(periodStartMinute + (DateTimeOffset.UtcNow - periodStartTime).TotalMinutes);

            if (soccerMatch.LastTimelineType.IsInjuryTimeShown || GetAnnouncedInjuryTime() > 0)
            {
                return BuildMinuteWithInjuryTime(matchMinute, periodEndMinute);
            }

            if (matchMinute >= periodEndMinute)
            {
                matchMinute = periodEndMinute;
            }

            if (matchMinute < periodStartMinute)
            {
                matchMinute = periodStartMinute;
            }

            Debug.WriteLine($"{match.Id}-{matchMinute}");
            return $"{matchMinute}'";

        }

        private string BuildMinuteWithInjuryTime(int matchMinute, int periodEndMinute)
        {
            var annoucedInjuryTime = GetAnnouncedInjuryTime();

            if (soccerMatch.InjuryTimeAnnounced > 0)
            {
                UpdateAnnouncedInjuryTime(soccerMatch.InjuryTimeAnnounced);
                annoucedInjuryTime = soccerMatch.InjuryTimeAnnounced;
            }

            var currentInjuryTime = matchMinute - periodEndMinute;
            var displayInjuryTime = currentInjuryTime <= 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime > annoucedInjuryTime)
            {
                displayInjuryTime = annoucedInjuryTime;
            }

            Debug.WriteLine($"{soccerMatch.Id}-{periodEndMinute}+{displayInjuryTime}'");
            return $"{periodEndMinute}+{displayInjuryTime}'";
        }

        private int GetAnnouncedInjuryTime()
        {
            var cacheKey = $"InjuryTimeAnnouced_{soccerMatch.Id}_{soccerMatch.MatchStatus.DisplayName}";

            // TODO: Should move InjuryTimeAnnouced to backend for storing?
            var annoucedInjuryTime = Task.Run(() => cachingService.GetOrCreateLocalMachine(cacheKey, 0)).Result;

            return annoucedInjuryTime;
        }

        public void UpdateAnnouncedInjuryTime(int injuryTime)
        {
            var cacheKey = $"InjuryTimeAnnouced_{soccerMatch.Id}_{soccerMatch.MatchStatus.DisplayName}";

            Task.Run(() => cachingService.InsertLocalMachine(cacheKey, injuryTime)).Wait();
        }
    }
}