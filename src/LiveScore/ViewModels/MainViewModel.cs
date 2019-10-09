using System;
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
        private readonly ILoggingService loggingService;

        public MainViewModel(
            ISettings settings,
            ICacheManager cacheManager,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            ILoggingService loggingService)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            this.settings = settings;
            this.cacheManager = cacheManager;
            this.loggingService = loggingService;
        
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);

            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            EventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
        }       

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private async Task Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
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
            try
            {
                if (!isConnected)
                {
                    await PopupNavigation.Instance.PushAsync(new NetworkConnectionError());
                }
                else
                {
                    if (PopupNavigation.Instance.PopupStack.Count > 0)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await loggingService.LogErrorAsync($"Error when receive OnConnectionChanged at {DateTime.Now}", ex);
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