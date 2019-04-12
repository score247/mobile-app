namespace LiveScore.Core.Controls.DateBar.Views
{
    using Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Models;
    using LiveScore.Core.Controls.DateBar.ViewModels;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        public DateBar()
        {
            InitializeComponent();

            ViewModel = new DateBarViewModel { HomeIsSelected = true };
            CalendarListView.BindingContext = ViewModel;
            HomeButton.BindingContext = ViewModel;
        }

        public DateBarViewModel ViewModel { get; set; }

        public static readonly BindableProperty NumberDisplayDaysProperty
          = BindableProperty.Create(
              nameof(NumberDisplayDays),
              typeof(int),
              typeof(DateBar),
              propertyChanged: OnNumberDisplayDaysPropertyChanged);

        public int NumberDisplayDays
        {
            get { return (int)GetValue(NumberDisplayDaysProperty); }
            set { SetValue(NumberDisplayDaysProperty, value); }
        }

        private static void OnNumberDisplayDaysPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateBar)bindable;

            if (control != null)
            {
                control.ViewModel.RenderCalendarItems((int)newValue);
            }
        }

        public static readonly BindableProperty SelectDateCommandProperty
           = BindableProperty.Create(
               nameof(SelectDateCommand),
               typeof(DelegateAsyncCommand<DateBarItem>),
               typeof(DateBar),
               propertyChanged: OnSelectDateCommandPropertyChanged);

        public DelegateAsyncCommand<DateBarItem> SelectDateCommand
        {
            get { return (DelegateAsyncCommand<DateBarItem>)GetValue(SelectDateCommandProperty); }
            set { SetValue(SelectDateCommandProperty, value); }
        }

        private static void OnSelectDateCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
                propertyChanged: OnSelectHomeCommandPropertyChanged);

        public DelegateAsyncCommand SelectHomeCommand
        {
            get { return (DelegateAsyncCommand)GetValue(SelectHomeCommandProperty); }
            set { SetValue(SelectHomeCommandProperty, value); }
        }

        private static void OnSelectHomeCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
