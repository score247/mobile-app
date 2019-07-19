namespace LiveScore.Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToApiFormat(this DateTime value)
            => value.ToString("yyyy-MM-ddTHH:mm:sszzz");

        public static string ToShortDayMonth(this DateTime value)
            => value.ToString("dd MMM");

        public static string ToTimeWithoutSecond(this DateTime value)
            => value.ToString("HH:mm");

        public static string ToDayMonthYear(this DateTime value)
            => value.ToString("dd MMM, yyyy");

        public static string ToFullDateTime(this DateTime value)
           => value.ToString("HH:mm dd MMM, yyyy");

        public static DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public static DateTime EndOfDay(this DateTime value) => value.AddDays(1).AddMilliseconds(-1);

        public static string ToDateAndTime(this DateTime value)
            => value.ToString("dd-MM HH:mm");
    }
}