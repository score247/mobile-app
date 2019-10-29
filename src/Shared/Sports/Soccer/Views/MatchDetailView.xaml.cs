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
                await MatchDetailLayout.TranslateTo(0, scrollY <= tabStripOffset ? -scrollY : -tabStripOffset, 0, Easing.Linear);
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<string, double>("TabListView", "OnScrolling");
        }
    }
}