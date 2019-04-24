namespace LiveScore.Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToApiFormat(this DateTime value)
            => value.ToString("yyyy-MM-ddTHH:mm:sszzz");

        public static string ToShortDayMonth(this DateTime value)
            => value.ToString("dd MMM");

        public static DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public static DateTime EndOfDay(this DateTime value) => value.AddDays(1).AddMilliseconds(-1);
    }
}