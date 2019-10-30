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
            AbsoluteLayout.SetLayoutBounds(MatchDetailLayout,
                new Rectangle(0, 0, 1, MatchDetailLayout.Height + tabStripOffset));
            AbsoluteLayout.SetLayoutFlags(MatchDetailLayout, AbsoluteLayoutFlags.WidthProportional);

            MessagingCenter.Subscribe<string, double>("TabListView", "OnScrolling", async (_, scrollY) =>
            {
                var newOffset = scrollY <= tabStripOffset ? scrollY : tabStripOffset;

                MatchDetailLayout.TranslationY = -newOffset;
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<string, double>("TabListView", "OnScrolling");
        }
    }
}