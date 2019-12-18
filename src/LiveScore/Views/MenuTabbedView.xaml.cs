using Prism;

namespace LiveScore.Views
{
    using Prism.Common;
    using Xamarin.Forms;
    using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
    using Xamarin.Forms.Xaml;

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
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);

            CurrentPageChanged += MenuTabbedView_CurrentPageChanged;
        }

        private void MenuTabbedView_CurrentPageChanged(object sender, System.EventArgs e)
        {
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