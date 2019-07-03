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
                if (scrollY <= TabStrip.Y)
                {
                    await ScrollView.ScrollToAsync(0, scrollY, false);
                }
                else
                {
                    await ScrollView.ScrollToAsync(0, TabStrip.Y, false);
                }
            });

            MessagingCenter.Subscribe<string>("MatchDetail", "OnScrollingBack", async (_) =>
            {
                await ScrollView.ScrollToAsync(0, 0, false);
            });
        }
    }
}