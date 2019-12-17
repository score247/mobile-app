using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.Calendar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : PopupPage
    {
        private readonly CalendarViewModel viewModel;

        public CalendarView(int marginTop = 0)
        {
            InitializeComponent();
            viewModel = new CalendarViewModel(2);

            BindingContext = viewModel;
            BackgroundInputTransparent = false;
            CloseWhenBackgroundIsClicked = true;
            Layout.Margin = new Thickness(0, marginTop, 0, 0);
        }
    }
}