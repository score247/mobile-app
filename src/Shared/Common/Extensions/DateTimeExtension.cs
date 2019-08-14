namespace LiveScore.Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToApiFormat(this DateTime value)
            => value.ToString("yyyy-MM-ddTHH:mm:sszzz");

        public static DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public static DateTime EndOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);

        public static DateTime EndOfDay(this DateTime date, int timeZoneOffset) 
            => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind)
                .AddHours(timeZoneOffset);

        public static DateTime BeginningOfDay(this DateTime date)
            => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);

        public static DateTime BeginningOfDay(this DateTime date, int timezoneOffset) 
            => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind)
                .AddHours(timezoneOffset);
    }

}
