namespace LiveScore.Core.Views
{
    using System;
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

        public event EventHandler CallbackEvent;

        private void InvoceCallback()
        {
            CallbackEvent?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            InvoceCallback();
        }

        private void SportOptionSelected(object sender, EventArgs e)
        {
            OnBackgroundClicked();
        }
    }
}