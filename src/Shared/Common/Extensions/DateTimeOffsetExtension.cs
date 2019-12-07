﻿using System;

namespace LiveScore.Common.Extensions
{
    public static class DateTimeOffsetExtension
    {
        public static string ToLocalShortDayMonth(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd MMM");

        public static string ToLocalTimeWithoutSecond(this DateTimeOffset value)
            => value.LocalDateTime.ToString("HH:mm");

        public static string ToLocalDayMonthYear(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd MMM, yyyy");

        public static string ToFullLocalDateTime(this DateTimeOffset value)
           => value.LocalDateTime.ToString("HH:mm dd MMM, yyyy");

        public static string ToLocalDateAndTime(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd-MM HH:mm");

        public static string ToFullLocalDateAndTime(this DateTimeOffset value)
           => value.LocalDateTime.ToString("yyyy-dd-MM HH:mm:ss");

        public static string ToLocalYear(this DateTimeOffset value)
           => value.LocalDateTime.Year.ToString();

        public static DateTimeOffset Yesterday() => DateTime.Today.AddDays(-1);

        public static bool IsFromYesterdayUntilNow(this DateTimeOffset dateTime)
           => dateTime <= DateTimeOffset.Now || dateTime >= Yesterday();

        public static string ToApiFormat(this DateTimeOffset value)
            => value.LocalDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}