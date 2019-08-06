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
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SetUpBarTextColor();
            }
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
        }

        private void SetUpBarTextColor()
        {
            var color = App.Current.Resources["FunctionBarActiveColor"];
            if (color != null)
            {
                BarTextColor = (Color)color;
            }
        }
    }
}