using Prism;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedView : Xamarin.Forms.TabbedPage
    {
        public MenuTabbedView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                SetUpBarTextColor();
            }

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
        }

        private void SetUpBarTextColor()
        {
            var color = PrismApplicationBase.Current.Resources["FunctionBarActiveColor"];
            if (color != null)
            {
                BarTextColor = (Color)color;
            }
        }
    }
}