using System;
using System.Globalization;

namespace LiveScore.Common.Extensions
{
    public static class DateTimeOffsetExtension
    {
        public static string ToDayMonth(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd MMM", CultureInfo.InvariantCulture);

        public static string ToMonthYear(this DateTimeOffset value)
            => value.LocalDateTime.ToString("MMM yyyy", CultureInfo.InvariantCulture);

        public static string ToTimeWithoutSecond(this DateTimeOffset value)
            => value.LocalDateTime.ToString("HH:mm", CultureInfo.InvariantCulture);

        public static string ToDayMonthYear(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);

        public static string ToDayMonthShortYear(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd MMM yy", CultureInfo.InvariantCulture);

        public static string ToDateTimeWithoutSecond(this DateTimeOffset value)
           => value.LocalDateTime.ToString("HH:mm dd MMM, yyyy", CultureInfo.InvariantCulture);

        public static string ToDateTimeWithoutYear(this DateTimeOffset value)
            => value.LocalDateTime.ToString("dd-MM HH:mm", CultureInfo.InvariantCulture);

        public static string ToFullDateTime(this DateTimeOffset value)
           => value.LocalDateTime.ToString("yyyy-dd-MM HH:mm:ss", CultureInfo.InvariantCulture);

        public static string ToLocalYear(this DateTimeOffset value)
           => value.LocalDateTime.Year.ToString();

        public static DateTimeOffset Yesterday() => DateTime.Today.AddDays(-1);

        public static bool IsFromYesterdayUntilNow(this DateTimeOffset dateTime)
           => dateTime <= DateTimeOffset.Now || dateTime >= Yesterday();

        public static string ToApiFormat(this DateTimeOffset value)
            => value.LocalDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);

        public static string ToH2HMatchShortDate(this DateTimeOffset value)
            => value.DateTime.Year == DateTime.Now.Year ? value.ToDayMonth() : value.ToLocalYear();

        public static string ToMatchShortDateMonth(this DateTimeOffset value)
            => value.DateTime.Year == DateTime.Now.Year ? value.ToDayMonth() : value.ToDayMonthShortYear();
    }
}