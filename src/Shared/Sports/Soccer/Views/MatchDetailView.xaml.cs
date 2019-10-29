using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views
{
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

            var tabStripOffset = TabStrip.Y;
            MessagingCenter.Subscribe<string, double>("TabListView", "OnScrolling", async (_, scrollY) =>
            {
                var newOffset = scrollY <= tabStripOffset ? scrollY : tabStripOffset;
                await MatchDetailLayout.LayoutTo(new Rectangle(0, -newOffset, MatchDetailLayout.Width, MatchDetailLayout.Height + newOffset), 0, Easing.Linear);
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<string, double>("TabListView", "OnScrolling");
        }
    }
}