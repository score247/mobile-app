using System;
using System.Threading.Tasks;
using LiveScore.Common.Services;
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
        private readonly ILoggingService loggingService;

        public SplashScreen(ILoggingService loggingService)
        {
            InitializeComponent();
            this.loggingService = loggingService;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await LoadMainPage().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await loggingService.LogErrorAsync(ex);
            }
        }

        [Time]
        private async Task LoadMainPage()
        {
            var mainPage = new MainView { Detail = new MenuTabbedView() };

            await PageUtilities.OnInitializedAsync(mainPage, null).ConfigureAwait(false);
            Navigation.InsertPageBefore(mainPage, Navigation.NavigationStack[0]);

            await Task.Delay(300);

            await Task.WhenAll(
                SplashIcon.Animate(new ScaleToAnimation { Scale = 0, Duration = "200", Easing = EasingType.Linear }),
                SplashIcon.Animate(new FadeToAnimation { Opacity = 0, Duration = "200", Easing = EasingType.Linear }));

            await Navigation.PopToRootAsync(false).ConfigureAwait(false);
        }
    }
}