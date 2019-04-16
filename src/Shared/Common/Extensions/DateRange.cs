﻿namespace LiveScore.Common.Extensions
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

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public static DateRange FromYesterdayUntilNow()
            => new DateRange(DateTime.Today.AddDays(-1), DateTime.Today);

        public static DateRange Now()
            => new DateRange(DateTime.Now);
    }
}