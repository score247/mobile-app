namespace LiveScore.Core.Controls.DateBar.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Models;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class DateBarViewModel
    {
        public ObservableCollection<DateBarItem> CalendarItems { get; set; }

        public DelegateAsyncCommand<DateBarItem> SelectDateCommand { get; set; }

        public bool HomeIsSelected { get; set; }

        public DelegateAsyncCommand SelectHomeCommand { get; set; }

        public void RenderCalendarItems(DateRange dateRange)
        {
            if (dateRange != null)
            {
                var dateItems = new List<DateBarItem>();

                for (var date = dateRange.FromDate; date <= dateRange.ToDate; date = date.AddDays(1))
                {
                    dateItems.Add(new DateBarItem { Date = date });
                }

                CalendarItems = new ObservableCollection<DateBarItem>(dateItems);
            }
        }

        public void InitSelectDateCommand(DelegateAsyncCommand<DateBarItem> command)
        {
            SelectDateCommand = new DelegateAsyncCommand<DateBarItem>(async (selectedDate) =>
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

        private void ReloadCalendarItems(DateBarItem selectedDate = null)
        {
            var calendarItems = CalendarItems;

            foreach (var item in calendarItems)
            {
                item.IsSelected = selectedDate != null
                        && selectedDate.Date.Day == item.Date.Day
                        && selectedDate.Date.Month == item.Date.Month
                        && selectedDate.Date.Year == item.Date.Year;
            }

            CalendarItems = new ObservableCollection<DateBarItem>(calendarItems);
        }
    }
}
