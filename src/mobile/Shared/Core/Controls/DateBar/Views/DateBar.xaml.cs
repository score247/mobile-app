namespace LiveScore.Core.Controls.DateBar.Views
{
    using System;
    using Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Controls.DateBar.ViewModels;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        public DateBar()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewModel = new DateBarViewModel { HomeIsSelected = true };
            CalendarListView.BindingContext = ViewModel;
            HomeButton.BindingContext = ViewModel;
        }

        public DateBarViewModel ViewModel { get; set; }

        public static readonly BindableProperty DateRangeProperty
            = BindableProperty.Create(
                nameof(DateRange),
                typeof(DateRange),
                typeof(DateBar),
                propertyChanged: OnDateRangeChanged);

        public DateRange DateRange
        {
            get { return (DateRange)GetValue(DateRangeProperty); }
            set { SetValue(DateRangeProperty, value); }
        }

        private static void OnDateRangeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateBar)bindable;

            if (control != null)
            {
                control.ViewModel.RenderCalendarItems((DateRange)newValue);
            }
        }

        public static readonly BindableProperty SelectDateCommandProperty
           = BindableProperty.Create(
               nameof(SelectDateCommand),
               typeof(DelegateAsyncCommand<DateBarItem>),
               typeof(DateBar),
               propertyChanged: OnSelectDateCommandChanged);

        public DelegateAsyncCommand<DateBarItem> SelectDateCommand
        {
            get { return (DelegateAsyncCommand<DateBarItem>)GetValue(SelectDateCommandProperty); }
            set { SetValue(SelectDateCommandProperty, value); }
        }

        private static void OnSelectDateCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateBar)bindable;

            if (control != null)
            {
                var command = (DelegateAsyncCommand<DateBarItem>)newValue;
                control.ViewModel.InitSelectDateCommand(command);
            }
        }

        public static readonly BindableProperty SelectHomeCommandProperty
            = BindableProperty.Create(
                nameof(SelectHomeCommand),
                typeof(DelegateAsyncCommand),
                typeof(DateBar),
                propertyChanged: OnSelectHomeCommandChanged);

        public DelegateAsyncCommand SelectHomeCommand
        {
            get { return (DelegateAsyncCommand)GetValue(SelectHomeCommandProperty); }
            set { SetValue(SelectHomeCommandProperty, value); }
        }

        private static void OnSelectHomeCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateBar)bindable;

            if (control != null)
            {
                var command = (DelegateAsyncCommand)newValue;
                control.ViewModel.InitSelectHomeCommand(command);
            }
        }
    }
}
