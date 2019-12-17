using System;
using System.Collections.Generic;

namespace LiveScore.Core.Controls.Calendar
{
    public class CalendarMonth
    {
        public CalendarMonth(IList<CalendarDates> calendarDates, int month, int year)
        {
            CalendarDates = calendarDates;
            Month = month;
            Year = year;
        }

        public int Month { get; }

        public int Year { get; }

        public IList<CalendarDates> CalendarDates { get; }
    }

    public class CalendarDates
    {
        public CalendarDates(CalendarDate[] dateList)
        {
            DateList = dateList;
        }

        public CalendarDate[] DateList { get; }
    }

    public class CalendarDate
    {
        public CalendarDate(DateTime date, bool isVisible = true)
        {
            Date = date;
            IsVisible = isVisible;
            IsCurrentDate = Date == DateTime.Today;
        }

        public bool IsVisible { get; }

        public bool IsCurrentDate { get; }

        public DateTime Date { get; }
    }
}