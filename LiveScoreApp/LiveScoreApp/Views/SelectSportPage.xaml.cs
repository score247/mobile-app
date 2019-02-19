namespace LiveScoreApp.Views
{
    using System;
    using Xamarin.Forms;

    public partial class SelectSportPage : ContentPage
    {
        public SelectSportPage()
        {
            this.InitComponent(InitializeComponent);
        }

        private async void OnClickDoneButton(object sender, EventArgs args) => await Navigation.PopModalAsync();
    }
}