using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.Calendar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        private readonly CalendarViewModel viewModel;

        public CalendarView()
        {
            InitializeComponent();
            viewModel = new CalendarViewModel(2);
            Layout.BindingContext = viewModel;
        }

        public static readonly BindableProperty DateSelectedCommandProperty
         = BindableProperty.Create(
             nameof(DateSelectedCommand),
             typeof(ICommand),
             typeof(CalendarView),
             propertyChanged: OnDateSelectedCommandChanged);

        public ICommand DateSelectedCommand
        {
            get => GetValue(DateSelectedCommandProperty) as ICommand;
            set => SetValue(DateSelectedCommandProperty, value);
        }

        private static void OnDateSelectedCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null && newValue != null)
            {
                var calendarView = bindable as CalendarView;

                if (calendarView.Layout.BindingContext is CalendarViewModel calendarViewModel)
                {
                    calendarViewModel.DateSelectedCommand = newValue as ICommand;
                }
            }
        }
    }
}