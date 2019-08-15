namespace LiveScore.Common.Extensions
{
    using System;

    public class DateRange
    {
        public DateRange()
            : this(DateTime.Today)
        {
        }

        public DateRange(DateTime datetime)
        {
            FromDate = datetime.BeginningOfDay();
            ToDate = datetime.EndOfDay();
        }

        public DateRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate.BeginningOfDay();
            ToDate = toDate.EndOfDay();
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string FromDateString => FromDate.ToApiFormat();

        public string ToDateString => ToDate.ToApiFormat();

        public bool IsOneDay => FromDate.Day == ToDate.Day;

        public static DateRange FromYesterdayUntilNow()
            => new DateRange(DateTime.Today.AddDays(-1).BeginningOfDay(), DateTime.Today.EndOfDay());

        public override string ToString() => $"{FromDateString}-{ToDateString}";

    }
}