namespace Score.Views
{
    using System;
    using CarouselView.FormsPlugin.Abstractions;
    using Score.Controls;
    using Score.Converters;
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
                throw ex;
            }
        }
    }
}