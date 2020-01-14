using System;

namespace LiveScore.Common.Extensions
{
#pragma warning disable S109 // Magic numbers should not be used

    public static class DateTimeExtension
    {
        public static string ToApiFormat(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");

        public static DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public static DateTime EndOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind);

        public static DateTime EndOfDay(this DateTime dateTime, int timeZoneOffset)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind)
                .AddHours(timeZoneOffset);

        public static DateTime BeginningOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);

        public static DateTime BeginningOfDay(this DateTime dateTime, int timezoneOffset)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind)
                .AddHours(timezoneOffset);

        public static bool IsTodayOrYesterday(this DateTime dateTime)
            => dateTime == DateTime.Today || dateTime == Yesterday();

        public static bool IsInDateRange(this DateTime dateTime, int range)
            => dateTime >= DateTime.Today.AddDays(-range) && dateTime <= DateTime.Today.AddDays(2);

        public static string ToFullDateTime(this DateTime value)
           => value.ToString("HH:mm dd MMM, yyyy");
    }

#pragma warning restore S109 // Magic numbers should not be used
}