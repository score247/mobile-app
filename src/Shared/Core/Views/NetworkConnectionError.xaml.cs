using System;
using LiveScore.Common.LangResources;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
            CloseWhenBackgroundIsClicked = true;
            BindingContext = this;
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        public string ErrorMessage => AppResources.ConnectionLostMessage;

        public async void OnTapped(object sender, EventArgs e)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}