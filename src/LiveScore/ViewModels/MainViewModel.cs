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
        private readonly ISettings settings;

        public MainViewModel(
            ICachingService cachingService,
            ISettings settings,
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.cachingService = cachingService;
            this.settings = settings;

            IsDemo = settings.IsDemo;
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
            settings.IsDemo = !settings.IsDemo;

            if (settings.IsDemo)
            {
                settings.ApiEndpoint = "https://score247-api1.nexdev.net/test/api/";
                settings.HubEndpoint = "https://score247-api2.nexdev.net/test/hubs/";
            }
            else
            {
#if DEBUG

                settings.ApiEndpoint = "https://score247-api1.nexdev.net/dev/api/";
                settings.HubEndpoint = "https://score247-api2.nexdev.net/dev/hubs/";
#else
                settings.ApiEndpoint = "https://score247-api1.nexdev.net/main/api/";
                settings.HubEndpoint = "https://score247-api2.nexdev.net/main/hubs/";
#endif
            }

            await CleanCacheAndRefreshCommand.ExecuteAsync();
        }

        private async Task CleanCacheAndRefresh()
        {
            //TODO implement clean cache with fanex caching
            //await cachingService.InvalidateAll();
            await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView));
        }
    }
}