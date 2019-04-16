namespace LiveScore.Core.Controls.DateBar.Views
{
    using LiveScore.Core.Controls.DateBar.ViewModels;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        public DateBar()
        {
            InitializeComponent();
            ViewModel = new DateBarViewModel
            {
                HomeIsSelected = true
            };

            CalendarListView.BindingContext = ViewModel;
            HomeButton.BindingContext = ViewModel;
        }

        protected override void OnBindingContextChanged()
        {
            var baseViewModel = (ViewModelBase)BindingContext;
            ViewModel.EventAggregator = baseViewModel.EventAggregator;
            ViewModel.SettingsService = baseViewModel.SettingsService;
            ViewModel.RenderCalendarItems();
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
                control.ViewModel.NumberOfDisplayDays = (int)newValue;
            }
        }
    }
}
