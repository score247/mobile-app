namespace LiveScore.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Common.Services;
    using LiveScore.Core;
    using LiveScore.Core.Services;
    using LiveScore.Views;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private readonly ICachingService cachingService;
        private readonly ISettingsService settingsService;

        public MainViewModel(
            ICachingService cachingService,
            ISettingsService settingsService,
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.cachingService = cachingService;
            this.settingsService = settingsService;

            IsDemo = settingsService.IsDemo;
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
            settingsService.IsDemo = !settingsService.IsDemo;

            if (settingsService.IsDemo)
            {
                settingsService.ApiEndpoint = "https://score247-api1.nexdev.net/test/api/";
                settingsService.HubEndpoint = "https://score247-api2.nexdev.net/test/hubs/";
            }
            else
            {
#if DEBUG

                settingsService.ApiEndpoint = "https://score247-api1.nexdev.net/dev/api/";
                settingsService.HubEndpoint = "https://score247-api2.nexdev.net/dev/hubs/";
#else
                settingsService.ApiEndpoint = "https://score247-api1.nexdev.net/main/api/";
                settingsService.HubEndpoint = "https://score247-api2.nexdev.net/main/hubs/";
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