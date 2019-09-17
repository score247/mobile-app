using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        private readonly INavigationService navigationService;

        public SplashScreen(INavigationService navigationService)
        {
            InitializeComponent();
            this.navigationService = navigationService;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ScaleIcon();
        }

        private async void ScaleIcon()
        {
            // wait until the UI is present
            await Task.Delay(300);

            // animate the splash logo
            await SplashIcon.ScaleTo(0.5, 500, Easing.CubicInOut);
            var animationTasks = new[]{
                SplashIcon.ScaleTo(100.0, 1000, Easing.CubicInOut),
                SplashIcon.FadeTo(0, 700, Easing.CubicInOut)
            };
            await Task.WhenAll(animationTasks);

            //// navigate to main page
            await navigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView), animated: false);
        }
    }
}