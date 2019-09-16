namespace LiveScore.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core;
    using Core.ViewModels;
    using Prism.Navigation;
    using Views;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private readonly ISettings settings;

        public MainViewModel(
            ISettings settings,
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.settings = settings;

            IsDemo = settings.IsDemo;
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
        }

        public bool IsDemo { get; set; }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        //TODO remove this command when ready to release
        public DelegateAsyncCommand ToggledCommand => new DelegateAsyncCommand(Toggle);

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private Task Navigate(string page)
        {
            return NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private Task Toggle()
        {
            settings.IsDemo = !settings.IsDemo;

            return CleanCacheAndRefreshCommand.ExecuteAsync();
        }

        private Task CleanCacheAndRefresh()
        {
            //TODO implement clean cache with fanex caching

            return NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView));
        }
    }
}