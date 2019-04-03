namespace Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToSportRadarFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        public static string ToShortDayMonth(this DateTime value) => value.ToString("dd MMM");
    }
}