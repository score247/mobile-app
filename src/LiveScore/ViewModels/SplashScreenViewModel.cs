namespace LiveScore.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Views;
    using Prism.Mvvm;
    using Prism.Navigation;

    public class SplashScreenViewModel : BindableBase, INavigationAware
    {
        private INavigationService NavigationService { get; }
        private IMatchService MatchService { get; }
        private Language CurrentLanguage { get; }

        public SplashScreenViewModel(INavigationService navigationService, IDependencyResolver dependencyResolver)
        {
            NavigationService = navigationService;
            MatchService = dependencyResolver.Resolve<IMatchService>();
            CurrentLanguage = AppSettings.Current.CurrentLanguage;
            MatchService.GetLiveMatches(CurrentLanguage, DateTime.Now);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            // Method intentionally left empty. (Requied by INavigationAware)
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(2000);
                await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView), animated: false).ConfigureAwait(true);
            });
        }
    }
}