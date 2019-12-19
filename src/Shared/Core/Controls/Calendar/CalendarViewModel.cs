using System;
using System.Collections.Generic;
using System.Windows.Input;
using PanCardView.EventArgs;
using Prism.Commands;
using PropertyChanged;

namespace LiveScore.Core.Controls.Calendar
{
    [AddINotifyPropertyChangedInterface]
    public class CalendarViewModel
    {
        private const int WeekDayCount = 7;
        private const string TitleDateFormat = "MMMM yyyy";
        private readonly int monthRange;

        public CalendarViewModel(int monthRange = 2)
        {
            this.monthRange = monthRange;
            CalendarMonthItemAppearingCommand = new DelegateCommand<ItemAppearingEventArgs>(OnCalendarMonthItemAppearing);
            SelectedDateChangedCommand = new DelegateCommand<CalendarDate>(OnChangeSelectedDate);
            PreviousMonthButtonTappedCommand = new DelegateCommand(OnTapPreviousMonth);
            NextMonthButtonTappedCommand = new DelegateCommand(OnTapNextMonth);
            BuildCalendarData(monthRange);
        }

        public int SelectedIndex { get; private set; }

        public IList<CalendarMonth> CalendarMonths { get; private set; }

        public string CalendarTitle { get; private set; }

        public CalendarDate SelectedDate { get; internal set; }

        public DelegateCommand<ItemAppearingEventArgs> CalendarMonthItemAppearingCommand { get; }

        public DelegateCommand<CalendarDate> SelectedDateChangedCommand { get; }

        public DelegateCommand PreviousMonthButtonTappedCommand { get; }

        public DelegateCommand NextMonthButtonTappedCommand { get; }

        public ICommand DateSelectedCommand { get; internal set; }

        private void BuildCalendarData(int monthRange)
        {
            CalendarTitle = BuildTitle(DateTime.Today.Year, DateTime.Today.Month);
            CalendarMonths = new List<CalendarMonth>();

            for (int monthIndex = -monthRange; monthIndex <= monthRange; monthIndex++)
            {
                var date = DateTime.Today.AddMonths(monthIndex);
                CalendarMonths.Add(new CalendarMonth(BuildCalendar(date.Year, date.Month), date.Month, date.Year));
            }

            SelectedIndex = monthRange;

            foreach (var calendarMonth in CalendarMonths)
            {
                foreach (var calendarDates in calendarMonth.CalendarDatesList)
                {
                    if (SelectedDate == null)
                    {
                        SelectedDate = Array.Find(calendarDates.DateList, date => date?.IsSelected == true);
                    }

                    calendarDates.SelectedDateChangedCommand = SelectedDateChangedCommand;
                }
            }
        }

        private void OnChangeSelectedDate(CalendarDate date)
        {
            if (SelectedDate == date)
            {
                return;
            }

            if (SelectedDate != null)
            {
                SelectedDate.IsSelected = false;
            }

            SelectedDate = date;
            DateSelectedCommand?.Execute(date);
        }

        private void OnCalendarMonthItemAppearing(ItemAppearingEventArgs obj)
        {
            var calendarMonth = obj.Item as CalendarMonth;
            CalendarTitle = BuildTitle(calendarMonth.Year, calendarMonth.Month);
        }

        private void OnTapPreviousMonth()
        {
            if (SelectedIndex > 0)
            {
                SelectedIndex--;
            }
        }

        private void OnTapNextMonth()
        {
            if (SelectedIndex < (monthRange * 2))
            {
                SelectedIndex++;
            }
        }

        private static string BuildTitle(int year, int month)
            => new DateTime(year, month, 1).ToString(TitleDateFormat);

        private static IList<CalendarDates> BuildCalendar(int year, int month)
        {
            var calendarDates = new List<CalendarDates>();
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstRow = new CalendarDate[WeekDayCount];
            var firstDayInMonth = new DateTime(year, month, 1);
            var firstDayInMonthWeekDay = firstDayInMonth.DayOfWeek;

            var firstRowDateCount = firstDayInMonthWeekDay == DayOfWeek.Sunday
                ? 1
                : Convert.ToInt32(WeekDayCount - firstDayInMonthWeekDay + 1);

            for (var i = 0; i < firstRowDateCount; i++)
            {
                firstRow[WeekDayCount - firstRowDateCount + i] = new CalendarDate(firstDayInMonth.AddDays(i));
            }

            calendarDates.Add(new CalendarDates(firstRow));

            var restRowsDateCount = daysInMonth - firstRowDateCount;
            var restRowsCount = restRowsDateCount / WeekDayCount;

            for (var i = 0; i < restRowsCount; i++)
            {
                var restRow = new CalendarDate[WeekDayCount];
                var firstDate = new DateTime(year, month, firstRowDateCount + 1).AddDays(WeekDayCount * i);

                for (var dateAdd = 0; dateAdd < WeekDayCount; dateAdd++)
                {
                    restRow[dateAdd] = new CalendarDate(firstDate.AddDays(dateAdd));
                }

                calendarDates.Add(new CalendarDates(restRow));
            }

            var lastRowDatesCount = restRowsDateCount % WeekDayCount;

            if (lastRowDatesCount > 0)
            {
                var lastRow = new CalendarDate[WeekDayCount];
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