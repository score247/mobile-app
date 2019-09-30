using Prism.Events;
using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public interface IConnectionStatusManager
    {
        void StartListen();
    }

    public class ConnectionChangePubSubEvent : PubSubEvent<bool>
    {
    }

    public class ConnectionStatusManager : IConnectionStatusManager
    {
        private readonly IEventAggregator eventAggregator;

        public ConnectionStatusManager(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void StartListen()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Publish(e.NetworkAccess == NetworkAccess.Internet);
        }
    }
}