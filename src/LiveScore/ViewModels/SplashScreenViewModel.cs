namespace LiveScore.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Views;
    using Prism.Mvvm;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class SplashScreenViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }
        private IMatchService MatchService { get; }
        private Language CurrentLanguage { get; }

        public SplashScreenViewModel(INavigationService navigationService, IDependencyResolver dependencyResolver)
        {
            NavigationService = navigationService;
            MatchService = dependencyResolver.Resolve<IMatchService>();
            CurrentLanguage = AppSettings.Current.CurrentLanguage;
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            var matches = await MatchService.GetMatchesByDate(
                  DateTime.Today,
                  CurrentLanguage,
                  true).ConfigureAwait(false);

            var navigationParams = new NavigationParameters();
            navigationParams.Add("Matches", matches);

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(2000);
                await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView), navigationParams, animated: false).ConfigureAwait(true);
            });
        }
    }
}