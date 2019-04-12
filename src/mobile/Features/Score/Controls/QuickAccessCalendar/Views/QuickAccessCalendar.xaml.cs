namespace LiveScore.Score.Controls.QuickAccessCalendar.Views
{
    using Common.Extensions;
    using LiveScore.Score.Controls.QuickAccessCalendar.Models;
    using LiveScore.Score.Controls.QuickAccessCalendar.ViewModels;
    using Xamarin.Forms;

    public partial class QuickAccessCalendar : ContentView
    {
        public QuickAccessCalendar()
        {
            InitializeComponent();
            ViewModel = new QuickAccessCalendarViewModel { HomeIsSelected = true };
            CalendarListView.BindingContext = ViewModel;
            HomeButton.BindingContext = ViewModel;
        }

        public QuickAccessCalendarViewModel ViewModel { get; set; }

        public static readonly BindableProperty DateRangeProperty
            = BindableProperty.Create(
                nameof(DateRange),
                typeof(DateRange),
                typeof(QuickAccessCalendar),
                propertyChanged: OnDateRangeChanged);

        public DateRange DateRange
        {
            get { return (DateRange)GetValue(DateRangeProperty); }
            set { SetValue(DateRangeProperty, value); }
        }

        private static void OnDateRangeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (QuickAccessCalendar)bindable;

            if (control != null)
            {
                control.ViewModel.RenderCalendarItems((DateRange)newValue);
            }
        }

        public static readonly BindableProperty SelectDateCommandProperty
           = BindableProperty.Create(
               nameof(SelectDateCommand),
               typeof(DelegateAsyncCommand<QuickAccessCalendarDate>),
               typeof(QuickAccessCalendar),
               propertyChanged: OnSelectDateCommandChanged);

        public DelegateAsyncCommand<QuickAccessCalendarDate> SelectDateCommand
        {
            get { return (DelegateAsyncCommand<QuickAccessCalendarDate>)GetValue(SelectDateCommandProperty); }
            set { SetValue(SelectDateCommandProperty, value); }
        }

        private static void OnSelectDateCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (QuickAccessCalendar)bindable;

            if (control != null)
            {
                var command = (DelegateAsyncCommand<QuickAccessCalendarDate>)newValue;
                control.ViewModel.InitSelectDateCommand(command);
            }
        }

        public static readonly BindableProperty SelectHomeCommandProperty
            = BindableProperty.Create(
                nameof(SelectHomeCommand),
                typeof(DelegateAsyncCommand),
                typeof(QuickAccessCalendar),
                propertyChanged: OnSelectHomeCommandChanged);

        public DelegateAsyncCommand SelectHomeCommand
        {
            get { return (DelegateAsyncCommand)GetValue(SelectHomeCommandProperty); }
            set { SetValue(SelectHomeCommandProperty, value); }
        }

        private static void OnSelectHomeCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (QuickAccessCalendar)bindable;

            if (control != null)
            {
                var command = (DelegateAsyncCommand)newValue;
                control.ViewModel.InitSelectHomeCommand(command);
            }
        }
    }
}
