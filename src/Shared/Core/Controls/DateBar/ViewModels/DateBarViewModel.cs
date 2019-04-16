namespace LiveScore.Core.Controls.DateBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Services;
    using Prism.Commands;
    using Prism.Events;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class DateBarViewModel
    {
        private DateBarItem currentDateBarItem;

        public DateBarViewModel()
        {
            SelectHomeCommand = new DelegateCommand(OnSelectHome);
            SelectDateCommand = new DelegateCommand<DateBarItem>(OnSelectDate);
        }

        public IEventAggregator EventAggregator { get; set; }

        public ISettingsService SettingsService { get; set; }

        public int NumberOfDisplayDays { get; set; }

        public bool HomeIsSelected { get; set; }

        public ObservableCollection<DateBarItem> CalendarItems { get; private set; }

        public DelegateCommand<DateBarItem> SelectDateCommand { get; }

        public DelegateCommand SelectHomeCommand { get; }

        public void RenderCalendarItems()
        {
            var dateItems = new List<DateBarItem>();

            for (var i = -NumberOfDisplayDays; i <= NumberOfDisplayDays; i++)
            {
                dateItems.Add(new DateBarItem { Date = DateTime.Today.ByTimeZone(SettingsService.CurrentTimeZone).AddDays(i) });
            }

            CalendarItems = new ObservableCollection<DateBarItem>(dateItems);
        }

        private void OnSelectDate(DateBarItem dateBarItem)
        {
            if (currentDateBarItem != dateBarItem)
            {
                currentDateBarItem = dateBarItem;
                HomeIsSelected = false;
                ReloadCalendarItems(dateBarItem);
                EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(new DateRange(dateBarItem.Date, dateBarItem.Date.EndDay()));
            }
        }

        private void OnSelectHome()
        {
            if (!HomeIsSelected)
            {
                currentDateBarItem = null;
                HomeIsSelected = true;
                ReloadCalendarItems();
                EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow(SettingsService.CurrentTimeZone));
            }
        }

        private void ReloadCalendarItems(DateBarItem dateBarItem = null)
        {
            var calendarItems = CalendarItems;

            foreach (var item in calendarItems)
            {
                item.IsSelected = dateBarItem != null
                        && dateBarItem.Date.Day == item.Date.Day
                        && dateBarItem.Date.Month == item.Date.Month
                        && dateBarItem.Date.Year == item.Date.Year;
            }

            CalendarItems = new ObservableCollection<DateBarItem>(calendarItems);
        }
    }
}
