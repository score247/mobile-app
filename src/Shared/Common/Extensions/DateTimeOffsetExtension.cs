using System;

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
    }
}