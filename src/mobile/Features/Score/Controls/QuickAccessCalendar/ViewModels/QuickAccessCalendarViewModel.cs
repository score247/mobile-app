namespace LiveScore.Score.Controls.QuickAccessCalendar.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LiveScore.Common.Extensions;
    using LiveScore.Score.Controls.QuickAccessCalendar.Models;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class QuickAccessCalendarViewModel
    {
        public ObservableCollection<QuickAccessCalendarDate> CalendarItems { get; set; }

        public DelegateAsyncCommand<QuickAccessCalendarDate> SelectDateCommand { get; set; }

        public bool HomeIsSelected { get; set; }

        public DelegateAsyncCommand SelectHomeCommand { get; set; }

        public void RenderCalendarItems(DateRange dateRange)
        {
            if (dateRange != null)
            {
                var dateItems = new List<QuickAccessCalendarDate>();

                for (var date = dateRange.FromDate; date <= dateRange.ToDate; date = date.AddDays(1))
                {
                    dateItems.Add(new QuickAccessCalendarDate { Date = date });
                }

                CalendarItems = new ObservableCollection<QuickAccessCalendarDate>(dateItems);
            }
        }

        public void InitSelectDateCommand(DelegateAsyncCommand<QuickAccessCalendarDate> command)
        {
            SelectDateCommand = new DelegateAsyncCommand<QuickAccessCalendarDate>(async (selectedDate) =>
            {
                HomeIsSelected = false;
                ReloadCalendarItems(selectedDate);
                await command?.ExecuteAsync(selectedDate);
            });
        }

        public void InitSelectHomeCommand(DelegateAsyncCommand command)
        {
            SelectHomeCommand = new DelegateAsyncCommand(async () =>
            {
                HomeIsSelected = true;
                ReloadCalendarItems();
                await command?.ExecuteAsync();
            });
        }

        private void ReloadCalendarItems(QuickAccessCalendarDate selectedDate = null)
        {
            var calendarItems = CalendarItems;

            foreach (var item in calendarItems)
            {
                item.IsSelected = selectedDate != null
                        && selectedDate.Date.Day == item.Date.Day
                        && selectedDate.Date.Month == item.Date.Month
                        && selectedDate.Date.Year == item.Date.Year;
            }

            CalendarItems = new ObservableCollection<QuickAccessCalendarDate>(calendarItems);
        }
    }
}
