namespace LiveScore.Core.Controls.DateBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Controls.DateBar.Models;
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

        public bool HomeIsSelected { get; set; }

        public ObservableCollection<DateBarItem> CalendarItems { get; private set; }

        public DelegateCommand<DateBarItem> SelectDateCommand { get; }

        public DelegateCommand SelectHomeCommand { get; }

        public void RenderCalendarItems(int numberDisplayDays)
        {
            var dateItems = new List<DateBarItem>();

            for (var i = -numberDisplayDays; i <= numberDisplayDays; i++)
            {
                dateItems.Add(new DateBarItem { Date = DateTime.Today.AddDays(i) });
            }

            CalendarItems = new ObservableCollection<DateBarItem>(dateItems);
        }

        private void OnSelectDate(DateBarItem selectedItem)
        {
            if (currentDateBarItem != selectedItem)
            {
                currentDateBarItem = selectedItem;
                EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(selectedItem.Date);
                HomeIsSelected = false;
                ReloadCalendarItems(selectedItem);
            }
        }

        private void OnSelectHome()
        {
            if (!HomeIsSelected)
            {
                EventAggregator.GetEvent<DateBarHomeSelectedEvent>().Publish();
                HomeIsSelected = true;
                ReloadCalendarItems();
            }
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
