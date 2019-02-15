using System;
using Xamarin.Forms;

namespace Setting.Views
{
    public partial class InfoAlertPage : ContentPage
    {
        public InfoAlertPage()
        {
            InitializeComponent();
        }

        private async void OnClickDoneButton(object sender, EventArgs args) => await Navigation.PopModalAsync();
    }
}