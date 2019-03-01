namespace LiveScoreApp.Views
{
    using System;
    using Xamarin.Forms;

    public partial class SelectSportPage : ContentPage
    {
        public SelectSportPage()
        {
#if DEBUG
            LiveReload.Init();
#endif
            InitializeComponent();
        }
    }
}