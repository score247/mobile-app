using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettings settings;
        private readonly ICacheManager cacheManager;

        public MainViewModel(
            ISettings settings,
            ICacheManager cacheManager,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            this.settings = settings;
            this.cacheManager = cacheManager;

            IsDemo = settings.IsDemo;
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);

            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            eventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
        }

        public bool IsDemo { get; set; }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        //TODO remove this command when ready to release
        public DelegateAsyncCommand ToggledCommand => new DelegateAsyncCommand(Toggle);

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private async Task Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private Task Toggle()
        {
            settings.IsDemo = !settings.IsDemo;

            return CleanCacheAndRefreshCommand.ExecuteAsync();
        }

        private async Task CleanCacheAndRefresh()
        {
            await cacheManager.InvalidateAll();

            await NavigateToHome();
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        private async void OnConnectionChanged(bool isConnected)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if(!isConnected)
            {
                await PopupNavigation.Instance.PushAsync(new NetworkConnectionError());
            }
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        private async void OnConnectionTimeout()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            await PopupNavigation.Instance.PushAsync(new NetworkConnectionError(AppResources.ConnectionTimeoutMessage));
        }

        public override void Destroy()
        {
            base.Destroy();

            EventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Unsubscribe(OnConnectionChanged);

            EventAggregator
                .GetEvent<ConnectionTimeoutPubSubEvent>()
                .Unsubscribe(OnConnectionTimeout);
        }
    }
}