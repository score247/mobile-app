namespace LiveScore.Core.Views
{
    using Rg.Plugins.Popup.Pages;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSportView : PopupPage
    {
        public SelectSportView()
        {
            InitializeComponent();

            BackgroundInputTransparent = true;
            BackgroundColor = Color.Transparent;
            CloseWhenBackgroundIsClicked = true;
        }
    }
}