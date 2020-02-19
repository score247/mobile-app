using System;
using System.Threading.Tasks;
using LiveScore.Core.Models.Notifications;
using LiveScore.Core.PubSubEvents.Notifications;
using Prism.Events;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPopupView : PopupPage
    {
        private const byte timeoutSeconds = 5;
        private readonly IEventAggregator eventAggregator;

        public NotificationPopupView(NotificationMessage notificationMessage, IEventAggregator eventAggregator)
        {
            NotificationMessage = notificationMessage;
            this.eventAggregator = eventAggregator;
            BackgroundInputTransparent = true;
            BackgroundColor = Color.Transparent;
            CloseWhenBackgroundIsClicked = true;
            AutoCloseMessageWhenTimeoutExpired();

            InitializeComponent();
            BindingContext = this;
        }

        public NotificationMessage NotificationMessage { get; }

        private void AutoCloseMessageWhenTimeoutExpired()
        {
            var self = this;
            Device.StartTimer(TimeSpan.FromSeconds(timeoutSeconds), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    await Device.InvokeOnMainThreadAsync(
                        async () => await PopupNavigation.Instance.RemovePageAsync(self));
                });

                return false;
            });
        }

        public async void OnTapped(object sender, EventArgs e)
        {
            eventAggregator.GetEvent<NotificationPubSubEvent>().Publish(NotificationMessage);
            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        public async void OnClosed(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}