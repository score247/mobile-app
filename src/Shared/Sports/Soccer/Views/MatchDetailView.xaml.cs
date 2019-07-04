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

            MessagingCenter.Subscribe<string, double>("MatchDetail", "OnScrolling", async (_, scrollY) =>
            {
                await ScrollView.ScrollToAsync(0, scrollY <= TabStrip.Y ? scrollY : TabStrip.Y, false);
            });
        }
    }
}