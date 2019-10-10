﻿using System;
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
        private readonly ICacheManager cacheManager;
        private readonly ILoggingService loggingService;

        public MainViewModel(
            ICacheManager cacheManager,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            ILoggingService loggingService)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            this.cacheManager = cacheManager;
            this.loggingService = loggingService;

            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);

            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            EventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private Task Navigate(string page)
        {
            return NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private async Task CleanCacheAndRefresh()
        {
            await cacheManager.InvalidateAll();

            await NavigateToHome();
        }

        private void OnConnectionChanged(bool isConnected)
        {
            try
            {
                if (!isConnected)
                {
                    PopupNavigation.Instance.PushAsync(new NetworkConnectionError());
                }
                else
                {
                    if (PopupNavigation.Instance.PopupStack.Count > 0)
                    {
                        PopupNavigation.Instance.PopAllAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                loggingService.LogErrorAsync($"Error when receive OnConnectionChanged at {DateTime.Now}", ex);
            }
        }

        private static void OnConnectionTimeout()
            => PopupNavigation.Instance.PushAsync(new NetworkConnectionError(AppResources.ConnectionTimeoutMessage));

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