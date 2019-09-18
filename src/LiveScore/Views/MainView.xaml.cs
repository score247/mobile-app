namespace LiveScore.Views
{
    using Prism.Navigation;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : IMasterDetailPageOptions
    {
        public MainView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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