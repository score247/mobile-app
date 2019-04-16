namespace LiveScore.Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToApiFormat(this DateTime value)
            => value.ToString("yyyy-MM-dd");

        public static string ToShortDayMonth(this DateTime value)
            => value.ToString("dd MMM");

        public static DateTime Yesteray(this DateTime value) => DateTime.Today.AddDays(-1);
    }
}