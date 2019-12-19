using Prism;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedView : Xamarin.Forms.TabbedPage
    {
        public MenuTabbedView()
        {
            // TODO: Remove this line when enable hamburger
            MenuTabbedView page = this;
            NavigationPage.SetHasNavigationBar(page, false);

            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SetUpBarTextColor();
            }
            On<Android>().SetIsSwipePagingEnabled(false);
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