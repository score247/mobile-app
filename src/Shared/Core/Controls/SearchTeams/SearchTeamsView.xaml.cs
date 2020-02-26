using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Controls.SearchTeams
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchTeamsView : ContentPage
    {
        public SearchTeamsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SearchBarControl.Focus();
        }

        private void ListView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            SearchBarControl.Unfocus();
        }
    }
}