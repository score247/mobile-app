namespace Common.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        public static string ToSportRadarFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }
    }
}