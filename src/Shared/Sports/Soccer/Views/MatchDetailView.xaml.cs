namespace LiveScore.Soccer.Views
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDetailView : ContentPage
    {
        public MatchDetailView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }
    }
}