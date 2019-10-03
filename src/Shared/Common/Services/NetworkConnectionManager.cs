using Prism.Events;
using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public interface INetworkConnectionManager
    {
        void StartListen();

        void PublishNetworkConnectionEvent();

        void PublishConnectionChangedEvent(NetworkAccess networkAccess);

        void PublishConnectionTimeoutEvent();

        NetworkAccess GetNetworkAccess();

        bool IsConnectionOK();

        bool IsConnectionNotOK();
    }

    public class ConnectionChangePubSubEvent : PubSubEvent<bool>
    {
    }

    public class ConnectionTimeoutPubSubEvent : PubSubEvent
    {
    }

    public class NetworkConnectionManager : INetworkConnectionManager
    {
        private readonly IEventAggregator eventAggregator;

        public NetworkConnectionManager(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public NetworkAccess GetNetworkAccess()
            => Connectivity.NetworkAccess;

        public bool IsConnectionNotOK()
            => !IsConnectionOK();

        public bool IsConnectionOK()
            => GetNetworkAccess() == NetworkAccess.Internet;

        public void PublishConnectionChangedEvent(NetworkAccess networkAccess)
            => eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Publish(networkAccess == NetworkAccess.Internet);

        public void PublishConnectionTimeoutEvent()
            => eventAggregator.GetEvent<ConnectionTimeoutPubSubEvent>().Publish();

        public void PublishNetworkConnectionEvent()
            => PublishConnectionChangedEvent(Connectivity.NetworkAccess);

        public void StartListen()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Publish(false);
            }

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            PublishConnectionChangedEvent(e.NetworkAccess);
        }
    }
}