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
            BindingContext = viewModel;
        }
    }
}