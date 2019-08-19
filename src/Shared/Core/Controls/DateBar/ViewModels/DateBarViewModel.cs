namespace LiveScore.Core.Controls.DateBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
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

        public DateTime CurrentTodayDate { get; } = DateTime.Today;

        public bool HomeIsSelected { get; set; }

        public IList<DateBarItem> CalendarItems { get; private set; }

        public DelegateCommand<DateBarItem> SelectDateCommand { get; }

        public DelegateCommand SelectHomeCommand { get; }

        public void InitializeBindingContext(object bindingContext)
        {
            var baseViewModel = (ViewModelBase)bindingContext;
            EventAggregator = baseViewModel.EventAggregator;
            SettingsService = baseViewModel.SettingsService;
            RenderCalendarItems();
            SelectHomeCommand?.Execute();
        }

        public void RenderCalendarItems()
        {
            var dateItems = new List<DateBarItem>();

            for (var i = -NumberOfDisplayDays; i <= NumberOfDisplayDays; i++)
            {
                dateItems.Add(new DateBarItem(DateTime.Today.AddDays(i)));
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
                EventAggregator
                    .GetEvent<DateBarItemSelectedEvent>()
                    .Publish(new DateRange(dateBarItem.Date, dateBarItem.Date.EndOfDay()));
            }
        }

        private void OnSelectHome()
        {
            if (!HomeIsSelected)
            {
                currentDateBarItem = null;
                HomeIsSelected = true;
                ReloadCalendarItems();
                EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());
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

            CalendarItems = new List<DateBarItem>(calendarItems);
        }
    }
}