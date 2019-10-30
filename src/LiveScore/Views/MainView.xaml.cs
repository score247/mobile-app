using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : IMasterDetailPageOptions
    {
        public MainView()
        {
            InitializeComponent();
#pragma warning disable S3366 // "this" should not be exposed from constructors
            NavigationPage.SetHasNavigationBar(this, false);
#pragma warning restore S3366 // "this" should not be exposed from constructors
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.iOS)
            {
                IsGestureEnabled = false;
            }
        }
    }
}