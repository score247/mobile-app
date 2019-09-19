using System.Threading.Tasks;
using Prism.Common;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        private const int milisecondsDelay = 300;

        public SplashScreen()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(milisecondsDelay);

            await LoadMainPage();
        }

        private async Task LoadMainPage()
        {
            var mainPage = new MainView { Detail = new MenuTabbedView() };
            await PageUtilities.OnInitializedAsync(mainPage, null);

            Navigation.InsertPageBefore(mainPage, Navigation.NavigationStack[0]);

            await Task.WhenAll(
                SplashIcon.Animate(new ScaleToAnimation { Scale = 0, Duration = "200", Easing = EasingType.Linear }),
                SplashIcon.Animate(new FadeToAnimation { Opacity = 0, Duration = "200", Easing = EasingType.Linear }));

            await Navigation.PopToRootAsync(false);
        }
    }
}