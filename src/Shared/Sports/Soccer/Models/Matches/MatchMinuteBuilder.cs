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

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public string BuildMatchMinute(IMatch match)
            => BuildMatchMinute(match as SoccerMatch, DateTimeOffset.UtcNow);

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        internal string BuildMatchMinute(SoccerMatch match, DateTimeOffset timeToViewMatch)
        {
            try
            {
                if (match == null || !PeriodTimes.ContainsKey(match.MatchStatus))
                {
                    return string.Empty;
                }

                var (periodStartMinute, periodEndMinute) = PeriodTimes[match.MatchStatus];

                var matchMinute = (int)(periodStartMinute + (timeToViewMatch - match.CurrentPeriodStartTime).TotalMinutes);

                if ((match.LastTimelineType?.IsInjuryTimeShown == true) || match.InjuryTimeAnnounced > 0)
                {
                    return BuildMinuteWithInjuryTime(matchMinute, periodEndMinute, match);
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
            catch
            {
                return string.Empty;
            }
        }

        private string BuildMinuteWithInjuryTime(int matchMinute, int periodEndMinute, SoccerMatch soccerMatch)
        {
            var announcedInjuryTime = soccerMatch.InjuryTimeAnnounced;

            var currentInjuryTime = matchMinute - periodEndMinute;
            var displayInjuryTime = currentInjuryTime <= 0 ? 1 : currentInjuryTime;

            if (currentInjuryTime > announcedInjuryTime)
            {
                displayInjuryTime = announcedInjuryTime;
            }

            return $"{periodEndMinute}+{displayInjuryTime}'";
        }
    }
}