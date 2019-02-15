namespace Setting.Views
{
    using System;
    using Xamarin.Forms;

    public partial class RefreshPage : ContentPage
    {
        public RefreshPage()
        {
            InitializeComponent();
        }

        private async void OnClickDoneButton(object sender, EventArgs args) => await Navigation.PopModalAsync();
    }
}