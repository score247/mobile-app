using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveScore.Core.Enumerations;

namespace Soccer.Tests.Services.Utils
{
    public static class MatchMinuteUtil
    {
        public static readonly ReadOnlyDictionary<MatchStatus, Tuple<byte, byte>> PeriodTimes
           = new ReadOnlyDictionary<MatchStatus, Tuple<byte, byte>>(
               new Dictionary<MatchStatus, Tuple<byte, byte>>
               {
                   [MatchStatus.FirstHalf] = new Tuple<byte, byte>(1, 45),
                   [MatchStatus.SecondHalf] = new Tuple<byte, byte>(46, 90),
                   [MatchStatus.FirstHalfExtra] = new Tuple<byte, byte>(91, 105),
                   [MatchStatus.SecondHalfExtra] = new Tuple<byte, byte>(106, 120)
               });

        private static readonly Random random = new Random();

        public static byte GenerateMatchMinute(MatchStatus matchStatus)
        {
            var (startTime, endTime) = PeriodTimes[matchStatus];

            return (byte)random.Next(startTime, endTime);
        }

        public static bool IsValidMatchMinuteDisplay(string actualDisplayMinute, byte expectedMinute, byte adjustableMinute = 1)
            => actualDisplayMinute?.Equals($"{expectedMinute + adjustableMinute}'", StringComparison.OrdinalIgnoreCase) == true
              || actualDisplayMinute?.Equals($"{expectedMinute}'", StringComparison.OrdinalIgnoreCase) == true;

        public static bool IsValidInjuryTimeDisplay(string actualDisplayMinute, byte periodEndMinute, byte minuteToViewMatchInjuiryTime = 1)
           => actualDisplayMinute?.Equals($"{periodEndMinute}+{minuteToViewMatchInjuiryTime}'", StringComparison.OrdinalIgnoreCase) == true
             || actualDisplayMinute?.Equals($"{periodEndMinute}+{minuteToViewMatchInjuiryTime + 1}'", StringComparison.OrdinalIgnoreCase) == true;
    }
}