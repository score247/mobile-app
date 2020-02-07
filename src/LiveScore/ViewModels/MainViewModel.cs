using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Features.Menu.Views;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly ILoggingService loggingService;
        private bool disposedValue;
        private readonly IAccountSettingService accountSettingsService;

        public MainViewModel(
        INavigationService navigationService,
        IDependencyResolver serviceLocator,
        IEventAggregator eventAggregator,
        ILoggingService loggingService)
        : base(navigationService, serviceLocator, eventAggregator)
        {
            this.loggingService = loggingService;
            accountSettingsService = serviceLocator.Resolve<AccountSettingsService>();
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            EventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
            GetAccountSettings();
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public bool NotificationStatus { get; set; }

        public void NotificationToggled(ToggledEventArgs arg)
        {
            accountSettingsService.UpdateNotificationStatus(arg.Value);
        }

        private void GetAccountSettings()
        {
            NotificationStatus = accountSettingsService.GetNotificationStatus();
        }

        private void OnConnectionChanged(bool isConnected)
        {
            try
            {
                if (!isConnected)
                {
                    PopupNavigation.Instance.PushAsync(new NetworkConnectionErrorPopupView());
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
                loggingService.LogExceptionAsync(ex, $"Error when receive OnConnectionChanged at {DateTime.Now}");
            }
        }

        private static void OnConnectionTimeout()
            => PopupNavigation.Instance.PushAsync(new NetworkConnectionErrorPopupView(AppResources.ConnectionTimeoutMessage));

        private static async Task Navigate(string page)
        {
            if (page == nameof(FAQView))
            {
                await Prism.PrismApplicationBase.Current.MainPage.Navigation.PushAsync(new FAQView());
            }
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Destroy();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}