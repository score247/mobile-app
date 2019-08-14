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

        public static DateTime EndOfDay(this DateTime dateTime, int timeZoneOffset) 
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind)
                .AddHours(timeZoneOffset);

        public static DateTime BeginningOfDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);

        public static DateTime BeginningOfDay(this DateTime dateTime, int timezoneOffset) 
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind)
                .AddHours(timezoneOffset);

        //public static bool EqualsWith(this DateTime dateTime1, DateTime dateTime2)
        //    => dateTime1.Date == dateTime2.Year && dateTime1.Month == dateTime2.Month;
    }

}
