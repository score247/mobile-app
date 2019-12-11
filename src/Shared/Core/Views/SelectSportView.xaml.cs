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

            BackgroundInputTransparent = false;
            BackgroundColor = Color.Transparent;
            CloseWhenBackgroundIsClicked = true;
        }

        protected override void OnDisappearing()
        {
            var t = this.Parent.BindingContext;
            base.OnDisappearing();
        }
    }
}