namespace LiveScore.Core.Controls.DateBar.Views
{
    using ViewModels;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        public DateBar()
        {
            InitializeComponent();
            ViewModel = new DateBarViewModel();

            CalendarListView.BindingContext = ViewModel;
            HomeButton.BindingContext = ViewModel;
        }

        protected override void OnBindingContextChanged()
        {
            ViewModel.InitializeBindingContext(BindingContext);
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
            get => (int)GetValue(NumberDisplayDaysProperty);
            set => SetValue(NumberDisplayDaysProperty, value);
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