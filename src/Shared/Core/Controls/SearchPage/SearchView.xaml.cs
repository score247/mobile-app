namespace LiveScore.Core.Controls.SearchPage
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchView : ContentPage
    {
        public SearchView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            searchTextBox.Focus();
        }
    }
}
