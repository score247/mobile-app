using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveScore.Core.Enumerations;

namespace Soccer.Tests.Services.Utils
{
    public static class MatchMinuteGenerator
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

        private static readonly Random random = new Random();

        public static byte RandomMinuteFor(MatchStatus matchStatus)
        {
            var (startTime, endTime) = PeriodTimes[matchStatus];

            return (byte)random.Next(startTime, endTime);
        }
    }
}