using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NetworkConnectionError : PopupPage
    {
        public NetworkConnectionError()
        {
            InitializeComponent();

            BackgroundInputTransparent = true;
            BackgroundColor = Color.Transparent;
        }
    }
}