using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Features.Menu.Views;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILoggingService loggingService;

        public MainViewModel(
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            ILoggingService loggingService)
            : base(navigationService, serviceLocator, eventAggregator)
        {
            this.loggingService = loggingService;
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            EventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

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
    }
}