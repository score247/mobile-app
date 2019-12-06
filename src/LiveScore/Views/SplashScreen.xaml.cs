using System.Threading.Tasks;
using MethodTimer;
using Prism.Common;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        private const int MillisecondsDelay = 1000;

        public SplashScreen()
        {
            InitializeComponent();

#pragma warning disable S3366 // "this" should not be exposed from constructors
            NavigationPage.SetHasNavigationBar(this, false);
#pragma warning restore S3366 // "this" should not be exposed from constructors
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadMainPageAsync();
        }

        [Time]
        private async Task LoadMainPageAsync()
        {
            // TODO: Change this line when enable hamburger
            //var mainPage = new MainView { Detail = new MenuTabbedView() };
            var mainPage = new MenuTabbedView();

            await PageUtilities.OnInitializedAsync(mainPage, null);
            Navigation.InsertPageBefore(mainPage, Navigation.NavigationStack[0]);

            await Task.Delay(MillisecondsDelay);

            await Task.WhenAll(SplashIcon.Animate(new ScaleToAnimation { Scale = 0, Duration = "200", Easing = EasingType.Linear }),
                        SplashIcon.Animate(new FadeToAnimation { Opacity = 0, Duration = "200", Easing = EasingType.Linear }));

            await Navigation.PopToRootAsync(false);
        }
    }
}