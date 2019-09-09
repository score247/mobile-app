namespace LiveScore.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using Common.Services;
    using Core;
    using Core.Services;
    using Views;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private readonly ICachingService cachingService;
        private readonly IAppSettings appSettings;

        public MainViewModel(
            ICachingService cachingService,
            IAppSettings appSettings,
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.cachingService = cachingService;
            this.appSettings = appSettings;

            IsDemo = appSettings.IsDemo;
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
        }

        public bool IsDemo { get; set; }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        //TODO remove this command when ready to release
        public DelegateAsyncCommand ToggledCommand => new DelegateAsyncCommand(Toogle);

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private async Task Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private async Task Toogle()
        {
            appSettings.IsDemo = !appSettings.IsDemo;

            if (appSettings.IsDemo)
            {
                appSettings.ApiEndpoint = "https://score247-api1.nexdev.net/test/api/";
                appSettings.HubEndpoint = "https://score247-api2.nexdev.net/test/hubs/";
            }
            else
            {
#if DEBUG

                appSettings.ApiEndpoint = "https://score247-api1.nexdev.net/dev/api/";
                appSettings.HubEndpoint = "https://score247-api2.nexdev.net/dev/hubs/";
#else
                appSettings.ApiEndpoint = "https://score247-api1.nexdev.net/main/api/";
                appSettings.HubEndpoint = "https://score247-api2.nexdev.net/main/hubs/";
#endif
            }

            await CleanCacheAndRefreshCommand.ExecuteAsync();
        }

        private async Task CleanCacheAndRefresh()
        {
            await cachingService.InvalidateAll();
            await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView));
        }
    }
}