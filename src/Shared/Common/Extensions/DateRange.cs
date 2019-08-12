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
            FromDate = ToDate = datetime;
        }

        public DateRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string FromDateString => FromDate.ToApiFormat();

        public string ToDateString => ToDate.ToApiFormat();

        public static DateRange FromYesterdayUntilNow()
            => new DateRange(DateTime.Today.AddDays(-1), DateTime.Today.EndOfDay());

        public override string ToString() => $"{FromDateString}-{ToDateString}";
        
    }
}