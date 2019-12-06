using Prism;

namespace LiveScore.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedView : Xamarin.Forms.TabbedPage
    {
        public MenuTabbedView()
        {
            // TODO: Remove this line when enable hamburger
            NavigationPage.SetHasNavigationBar(this, false);

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