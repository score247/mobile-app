using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Soccer.Models.Matches
{
    // TODO: Unit test will be written in Performance Enhancement branch
    public class MatchMinuteBuilder : IMatchMinuteBuilder
    {
        private static readonly ReadOnlyDictionary<MatchStatus, Tuple<byte, byte>> PeriodTimes
          = new ReadOnlyDictionary<MatchStatus, Tuple<byte, byte>>(
              new Dictionary<MatchStatus, Tuple<byte, byte>>
              {
                  [MatchStatus.FirstHalf] = new Tuple<byte, byte>(1, 45),
                  [MatchStatus.SecondHalf] = new Tuple<byte, byte>(46, 90),
                  [MatchStatus.FirstHalfExtra] = new Tuple<byte, byte>(91, 105),
                  [MatchStatus.SecondHalfExtra] = new Tuple<byte, byte>(106, 120)
              });

        private readonly ISettings settings;
        private readonly ILoggingService loggingService;

        public MatchMinuteBuilder(ISettings settings, ILoggingService loggingService)
        {
            this.settings = settings;
            this.loggingService = loggingService;
        }

        // TODO: Remove casting
        public string BuildMatchMinute(IMatch match) => BuildMatchMinute(match as SoccerMatch, DateTimeOffset.UtcNow);

        internal string BuildMatchMinute(SoccerMatch soccerMatch, DateTimeOffset queryTime)
        {
            try
            {
                if (soccerMatch == null)
                {
                    return string.Empty;
                }

                var (periodStartMinute, periodEndMinute) = PeriodTimes[soccerMatch.MatchStatus];

                var periodStartTime = soccerMatch.CurrentPeriodStartTime == DateTimeOffset.MinValue
                    ? soccerMatch.EventDate
                    : soccerMatch.CurrentPeriodStartTime;

                // TODO: What if CurrentPeriodStartTime does not have data?
                var matchMinute = (int)(periodStartMinute + (queryTime - periodStartTime).TotalMinutes);

                if ((soccerMatch.LastTimelineType?.IsInjuryTimeShown == true) || GetAnnouncedInjuryTime(soccerMatch) > 0)
                {
                    return BuildMinuteWithInjuryTime(matchMinute, periodEndMinute, soccerMatch);
                }

                if (matchMinute >= periodEndMinute)
                {
                    matchMinute = periodEndMinute;
                }

                if (matchMinute < periodStartMinute)
                {
                    matchMinute = periodStartMinute;
                }

                return $"{matchMinute}'";
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex);

                return string.Empty;
            }
        }

        private string BuildMinuteWithInjuryTime(int matchMinute, int periodEndMinute, SoccerMatch soccerMatch)
        {
            var annoucedInjuryTime = GetAnnouncedInjuryTime(soccerMatch);

            if (soccerMatch.InjuryTimeAnnounced > 0)
            {
                UpdateAnnouncedInjuryTime(soccerMatch);
                annoucedInjuryTime = soccerMatch.InjuryTimeAnnounced;
            }

            var currentInjuryTime = matchMinute - periodEndMinute;
            var displayInjuryTime = currentInjuryTime <= 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime > annoucedInjuryTime)
            {
                displayInjuryTime = annoucedInjuryTime;
            }

            return $"{periodEndMinute}+{displayInjuryTime}'";
        }

        private int GetAnnouncedInjuryTime(SoccerMatch soccerMatch)
        {
            // TODO: Should move InjuryTimeAnnouced to backend for storing?
            var cachedInjuryTime = settings.Get(GetCacheKey(soccerMatch));

            return string.IsNullOrWhiteSpace(cachedInjuryTime) ? 0 : int.Parse(cachedInjuryTime);
        }

        public void UpdateAnnouncedInjuryTime(SoccerMatch soccerMatch)
        {
            settings.Set(GetCacheKey(soccerMatch), soccerMatch.InjuryTimeAnnounced.ToString());
        }

        private static string GetCacheKey(SoccerMatch soccerMatch) => $"InjuryTimeAnnouced_{soccerMatch.Id}_{soccerMatch.MatchStatus.DisplayName}";
    }
}