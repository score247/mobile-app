namespace LiveScore.Views
{
    using System.Threading.Tasks;
    using Prism.Common;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // wait until the UI is present
            await Task.Delay(500);

            await LoadMainPage();

        }

        private async Task LoadMainPage()
        {
            var mainPage = new MainView { Detail = new MenuTabbedView() };
            await PageUtilities.OnInitializedAsync(mainPage, null);

            Navigation.InsertPageBefore(mainPage, Navigation.NavigationStack[0]);

            await Navigation.PopToRootAsync(false);
        }
    }
}