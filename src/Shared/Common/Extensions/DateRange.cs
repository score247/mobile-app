namespace LiveScore.Common.Extensions
{
    using System;

    public struct DateRange : IEquatable<DateRange>
    {
        public DateRange(DateTime datetime)
        {
            From = datetime.BeginningOfDay();
            To = datetime.EndOfDay();
        }

        public DateRange(DateTime fromDate, DateTime toDate)
        {
            From = fromDate.BeginningOfDay();
            To = toDate.EndOfDay();
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public string FromDateString => From.ToApiFormat();

        public string ToDateString => To.ToApiFormat();

        public bool IsOneDay => Days == 1;

        public int Days => (From - To).Days + 1;

        public static DateRange FromYesterdayUntilNow()
            => new DateRange(DateTime.Today.AddDays(-1).BeginningOfDay(), DateTime.Today.EndOfDay());

        public bool Equals(DateRange other)
            => other.From.Date == From.Date && other.To.Date == To.Date;

        public override string ToString() => $"{FromDateString}-{ToDateString}";
    }
}