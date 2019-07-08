namespace LiveScore.Soccer.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchDetailView : ContentPage
    {
        public MatchDetailView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, double>("TabListView", "OnScrolling", async (_, scrollY) =>
            {
                await ScrollView.ScrollToAsync(0, scrollY <= TabStrip.Y ? scrollY : TabStrip.Y, false);
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<string, double>("TabListView", "OnScrolling");
        }
    }
}