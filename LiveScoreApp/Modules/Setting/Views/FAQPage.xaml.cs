using System;
using Xamarin.Forms;

namespace Setting.Views
{
    public partial class FAQPage : ContentPage
    {
        public FAQPage()
        {
            InitializeComponent();
        }

        private async void OnClickDoneButton(object sender, EventArgs args) => await Navigation.PopModalAsync();
    }
}