using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Features.Menu.Views;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly ILoggingService loggingService;
        private readonly ISettingsService settingsService;
        private readonly IFavoriteService<IMatch> favoriteMatchService;
        private readonly IFavoriteService<ILeague> favoriteLeagueService;
        private bool disposedValue;

        public MainViewModel(
        INavigationService navigationService,
        IDependencyResolver serviceLocator,
        IEventAggregator eventAggregator,
        ILoggingService loggingService)
        : base(navigationService, serviceLocator, eventAggregator)
        {
            this.loggingService = loggingService;
            settingsService = serviceLocator.Resolve<ISettingsService>();
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
            EventAggregator.GetEvent<ConnectionChangePubSubEvent>().Subscribe(OnConnectionChanged);
            EventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Subscribe(OnConnectionTimeout);
            NotificationStatus = settingsService.GetNotificationStatus();
            favoriteMatchService = DependencyResolver.Resolve<IFavoriteService<IMatch>>(CurrentSportId.ToString());
            favoriteLeagueService = DependencyResolver.Resolve<IFavoriteService<ILeague>>(CurrentSportId.ToString());
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public bool NotificationStatus { get; set; }


        public void NotificationToggled(ToggledEventArgs arg)
        {
            settingsService.UpdateNotificationStatus(arg.Value);
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
                    favoriteMatchService.Sync();
                    favoriteLeagueService.Sync();
                    settingsService.SyncNotification();

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
            else if (page == nameof(AboutScore247View))
            {
                await Prism.PrismApplicationBase.Current.MainPage.Navigation.PushAsync(new AboutScore247View());
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