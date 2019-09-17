using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Services;
using LiveScore.Features.Score.Views;
using LiveScore.Soccer.Models.Matches;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        private readonly INavigationService navigationService;
        private readonly IMatchService matchService;
        private readonly ISettings settings;

        public SplashScreen(INavigationService navigationService, IDependencyResolver dependencyResolver)
        {
            InitializeComponent();
            this.navigationService = navigationService;
            settings = dependencyResolver.Resolve<ISettings>();
            matchService = dependencyResolver.Resolve<IMatchService>(settings.CurrentSportType.Value.ToString());

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var task = matchService.GetMatchesByDate(
                      DateTime.Today,
                      settings.CurrentLanguage,
                      true);

            await Task.WhenAll(task, ScaleIcon());
        }

        private async Task ScaleIcon()
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
            ///
            await navigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView), animated: false);
        }
    }
}