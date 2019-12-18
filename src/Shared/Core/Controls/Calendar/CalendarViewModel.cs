using System;
using System.Collections.Generic;
using PanCardView.EventArgs;
using Prism.Commands;
using PropertyChanged;

namespace LiveScore.Core.Controls.Calendar
{
    [AddINotifyPropertyChangedInterface]
    public class CalendarViewModel
    {
        public CalendarViewModel(int monthRange = 2)
        {
            CalendarMonthItemAppearingCommand = new DelegateCommand<ItemAppearingEventArgs>(OnCalendarMonthItemAppearing);
            CalendarTitle = DateTime.Today.ToString("MMMM yyyy");
            CalendarMonths = new List<CalendarMonth>();

            for (int monthIndex = -monthRange; monthIndex <= monthRange; monthIndex++)
            {
                var date = DateTime.Today.AddMonths(monthIndex);
                CalendarMonths.Add(new CalendarMonth(BuildCalendar(date.Year, date.Month), date.Month, date.Year));
            }

            SelectedIndex = monthRange;

            ChangeSelectedDateCommand = new DelegateCommand<CalendarDate>(OnChangeSelectedDate);

            foreach (var calendarMonth in CalendarMonths)
            {
                foreach (var calendarDates in calendarMonth.CalendarDatesList)
                {
                    if (SelectedDate == null)
                    {
                        SelectedDate = Array.Find(calendarDates.DateList, date => date?.IsSelected == true);
                    }

                    calendarDates.ChangeSelectedDateCommand = ChangeSelectedDateCommand;
                }
            }
        }

        public int SelectedIndex { get; }

        public IList<CalendarMonth> CalendarMonths { get; }

        public string CalendarTitle { get; private set; }

        public CalendarDate SelectedDate { get; set; }

        public DelegateCommand<ItemAppearingEventArgs> CalendarMonthItemAppearingCommand { get; }

        public DelegateCommand<CalendarDate> ChangeSelectedDateCommand { get; }

        private void OnChangeSelectedDate(CalendarDate date)
        {
            if (SelectedDate != null)
            {
                SelectedDate.IsSelected = false;
            }

            SelectedDate = date;
        }

        private void OnCalendarMonthItemAppearing(ItemAppearingEventArgs obj)
        {
            var calendarMonth = obj.Item as CalendarMonth;
            CalendarTitle = new DateTime(calendarMonth.Year, calendarMonth.Month, 1).ToString("MMMM yyyy");
        }

        private static IList<CalendarDates> BuildCalendar(int year, int month)
        {
            var calendarDates = new List<CalendarDates>();
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstRow = new CalendarDate[7];
            var firstDayInMonth = new DateTime(year, month, 1);
            var firstDayInMonthWeekDay = firstDayInMonth.DayOfWeek;

            var firstRowDateCount = firstDayInMonthWeekDay == DayOfWeek.Sunday
                ? 1
                : Convert.ToInt32(7 - firstDayInMonthWeekDay + 1);

            for (var i = 0; i < firstRowDateCount; i++)
            {
                firstRow[7 - firstRowDateCount + i] = new CalendarDate(firstDayInMonth.AddDays(i));
            }

            calendarDates.Add(new CalendarDates(firstRow));

            var restRowsDateCount = daysInMonth - firstRowDateCount;
            var restRowsCount = restRowsDateCount / 7;

            for (var i = 0; i < restRowsCount; i++)
            {
                var restRow = new CalendarDate[7];
                var firstDate = new DateTime(year, month, firstRowDateCount + 1).AddDays(7 * i);

                for (var dateAdd = 0; dateAdd < 7; dateAdd++)
                {
                    restRow[dateAdd] = new CalendarDate(firstDate.AddDays(dateAdd));
                }

                calendarDates.Add(new CalendarDates(restRow));
            }

            var lastRowDatesCount = restRowsDateCount % 7;

            if (lastRowDatesCount > 0)
            {
                var lastRow = new CalendarDate[7];
                var firstDateInLastRow = new DateTime(year, month, daysInMonth - lastRowDatesCount + 1);

                for (var i = 0; i < lastRowDatesCount; i++)
                {
                    lastRow[i] = new CalendarDate(firstDateInLastRow.AddDays(i));
                }

                calendarDates.Add(new CalendarDates(lastRow));
            }

            return calendarDates;
        }
    }
}