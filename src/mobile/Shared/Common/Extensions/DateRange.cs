namespace LiveScore.Common.Extensions
{
    using System;

    public class DateRange
    {
        /// <summary>
        /// Default date range: From is Yesterday, and To is tomorrow
        /// </summary>
        public DateRange()
            : this(DateTime.Today.AddDays(-1), DateTime.Today)
        {
        }

        public DateRange(DateTime date)
        {
            FromDate = ToDate = date;
        }

        public DateRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}