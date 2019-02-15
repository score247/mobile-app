using System;
using Xamarin.Forms;

namespace Setting.Views
{
    public partial class DefaultLanguagePage : ContentPage
    {
        public DefaultLanguagePage()
        {
            InitializeComponent();
        }

        private async void OnClickDoneButton(object sender, EventArgs args) => await Navigation.PopModalAsync();
    }
}