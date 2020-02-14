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
            InitializeComponent();
            InitConnectionLostMessage();
        }

        private void InitConnectionLostMessage()
        {
            BackgroundInputTransparent = true;
            BackgroundColor = Color.Transparent;
            CloseWhenBackgroundIsClicked = true;
            BindingContext = this;
            AutoCloseMessageWhenTimeoutExpired();
        }

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

        public NotificationMessage NotificationMessage { get; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async void OnTapped(object sender, EventArgs e)
        {
            eventAggregator.GetEvent<NotificationPubSubEvent>().Publish(NotificationMessage);
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}