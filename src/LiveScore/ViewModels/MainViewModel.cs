using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICacheManager cacheManager;

        public MainViewModel(
            ICacheManager cacheManager,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            this.cacheManager = cacheManager;

            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private Task Navigate(string page)
        {
            return NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private async Task CleanCacheAndRefresh()
        {
            await cacheManager.InvalidateAllAsync();

            await NavigateToHomeAsync();
        }
    }
}