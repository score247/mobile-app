namespace LiveScore.Core.Controls.DateBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Events;
    using Models;
    using Services;
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

        public IAppSettings AppSettings { get; set; }

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
            AppSettings = baseViewModel.AppSettings;
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

            CalendarItems = new List<DateBarItem>(dateItems);
        }

        private void OnSelectDate(DateBarItem dateBarItem)
        {
            if (currentDateBarItem != dateBarItem)
            {
                currentDateBarItem = dateBarItem;
                HomeIsSelected = false;
                SetSelectedCalendarItems(dateBarItem);

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
                SetSelectedCalendarItems();

                EventAggregator
                    .GetEvent<DateBarItemSelectedEvent>()
                    .Publish(DateRange.FromYesterdayUntilNow());
            }
        }

        // TODo: do we really need it
        private void SetSelectedCalendarItems(DateBarItem dateBarItem = null)
        {
            var calendarItems = CalendarItems;

            foreach (var item in CalendarItems)
            {
                item.IsSelected = dateBarItem != null && item.Equals(dateBarItem);
            }

            CalendarItems = new List<DateBarItem>(calendarItems);
        }
    }
}