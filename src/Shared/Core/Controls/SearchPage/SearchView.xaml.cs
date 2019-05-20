namespace LiveScore.Core.Controls.SearchPage
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchView : ContentPage
    {
        public SearchView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            searchTextBox.Focus();
        }
    }
}
