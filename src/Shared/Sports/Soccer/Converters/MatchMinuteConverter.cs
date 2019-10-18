﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Converters
{
    // TODO: Unit test will be written in Performance Enhancement branch
    public class MatchMinuteConverter : IMatchMinuteBuilder
    {
        private static readonly ReadOnlyDictionary<MatchStatus, int> PeriodStartMinutes
            = new ReadOnlyDictionary<MatchStatus, int>(
                new Dictionary<MatchStatus, int>
                {
                    { MatchStatus.FirstHalf, 1 },
                    { MatchStatus.SecondHalf, 46 },
                    { MatchStatus.FirstHalfExtra, 91 },
                    { MatchStatus.SecondHalfExtra, 106 }
                });

        private static readonly ReadOnlyDictionary<MatchStatus, int> PeriodEndMinutes
            = new ReadOnlyDictionary<MatchStatus, int>(
                new Dictionary<MatchStatus, int>
                {
                    { MatchStatus.FirstHalf, 45 },
                    { MatchStatus.SecondHalf, 90 },
                    { MatchStatus.FirstHalfExtra, 105 },
                    { MatchStatus.SecondHalfExtra, 120 }
                });

        private readonly ISettings settings;
        private readonly ILoggingService loggingService;

        private SoccerMatch soccerMatch;

        public MatchMinuteConverter(ISettings settings, ILoggingService loggingService)
        {
            this.settings = settings;
            this.loggingService = loggingService;
        }

        public string BuildMatchMinute(IMatch match) => BuildMatchMinute(match, DateTimeOffset.UtcNow);

        internal string BuildMatchMinute(IMatch match, DateTimeOffset queryTime)
        {
            try
            {
                if (!(match is SoccerMatch) || match == null)
                {
                    return string.Empty;
                }

                soccerMatch = match as SoccerMatch;

                PeriodStartMinutes.TryGetValue(match.MatchStatus, out var periodStartMinute);
                PeriodEndMinutes.TryGetValue(match.MatchStatus, out var periodEndMinute);

                var periodStartTime = soccerMatch.CurrentPeriodStartTime == DateTimeOffset.MinValue
                    ? soccerMatch.EventDate
                    : soccerMatch.CurrentPeriodStartTime;

                // TODO: What if CurrentPeriodStartTime does not have data?
                var matchMinute = (int)(periodStartMinute + (queryTime - periodStartTime).TotalMinutes);

                if ((soccerMatch.LastTimelineType?.IsInjuryTimeShown == true) || GetAnnouncedInjuryTime() > 0)
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

                return $"{matchMinute}'";
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex);

                return string.Empty;
            }
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

            return $"{periodEndMinute}+{displayInjuryTime}'";
        }

        private int GetAnnouncedInjuryTime()
        {
            // TODO: Should move InjuryTimeAnnouced to backend for storing?
            var cachedInjuryTime = settings.Get(CacheKey);

            return string.IsNullOrWhiteSpace(cachedInjuryTime) ? 0 : int.Parse(cachedInjuryTime);
        }

        public void UpdateAnnouncedInjuryTime(int injuryTime)
        {
            settings.Set(CacheKey, injuryTime.ToString());
        }

        private string CacheKey => $"InjuryTimeAnnouced_{soccerMatch.Id}_{soccerMatch.MatchStatus.DisplayName}";
    }
}