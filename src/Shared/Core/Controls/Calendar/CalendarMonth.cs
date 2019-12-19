using System;
using System.Collections.Generic;
using Prism.Commands;
using PropertyChanged;

namespace LiveScore.Core.Controls.Calendar
{
    [AddINotifyPropertyChangedInterface]
    public class CalendarMonth
    {
        public CalendarMonth(IList<CalendarDates> calendarDatesList, int month, int year)
        {
            CalendarDatesList = calendarDatesList;
            Month = month;
            Year = year;
        }

        public int Month { get; }

        public int Year { get; }

        public IList<CalendarDates> CalendarDatesList { get; }
    }

    [AddINotifyPropertyChangedInterface]
    public class CalendarDates
    {
        public CalendarDates(CalendarDate[] dateList)
        {
            DateList = dateList;
            DateSelectedCommand = new DelegateCommand<CalendarDate>(OnSelectDate);
        }

        public CalendarDate[] DateList { get; }

        public DelegateCommand<CalendarDate> DateSelectedCommand { get; }

        public DelegateCommand<CalendarDate> SelectedDateChangedCommand { get; set; }

        private void OnSelectDate(CalendarDate date)
        {
            if (date != null)
            {
                date.IsSelected = true;
                SelectedDateChangedCommand?.Execute(date);
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class CalendarDate
    {
        public CalendarDate(DateTime date, bool isVisible = true)
        {
            Date = date;
            IsVisible = isVisible;
            IsSelected = Date == DateTime.Today;
            IsToday = Date == DateTime.Today;
        }

        public bool IsVisible { get; }

        public bool IsSelected { get; set; }

        public bool IsToday { get; }

        public DateTime Date { get; }
    }
}