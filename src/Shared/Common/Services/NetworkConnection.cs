using System;
using Prism.Events;
using Xamarin.Essentials;

namespace LiveScore.Common.Services
{
    public class ConnectionChangePubSubEvent : PubSubEvent<bool>
    {
    }

    public class ConnectionTimeoutPubSubEvent : PubSubEvent
    {
    }

    public interface INetworkConnection
    {
        void StartListen();

        void PublishNetworkConnectionEvent();

        void PublishConnectionChangedEvent(NetworkAccess networkAccess);

        void PublishConnectionTimeoutEvent();

        NetworkAccess GetNetworkAccess();

        bool IsSuccessfulConnection();

        bool IsFailureConnection();
    }

    public class NetworkConnection : INetworkConnection
    {
        private readonly IEventAggregator eventAggregator;

        public NetworkConnection(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public NetworkAccess GetNetworkAccess()
            => Connectivity.NetworkAccess;

        public bool IsFailureConnection()
            => !IsSuccessfulConnection();

        public bool IsSuccessfulConnection()
            => GetNetworkAccess() == NetworkAccess.Internet;

        public void PublishConnectionChangedEvent(NetworkAccess networkAccess)
            => eventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Publish(networkAccess == NetworkAccess.Internet);

        public void PublishConnectionTimeoutEvent()
            => eventAggregator
                .GetEvent<ConnectionTimeoutPubSubEvent>()
                .Publish();

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

    public static class NetworkConnectionExtensions
    {
        public static INetworkConnection OnSuccessfulConnection(
            this INetworkConnection networkConnection, Action action)
        {
            if (networkConnection.IsSuccessfulConnection() && action != null)
            {
                action();
            }

            return networkConnection;
        }

        public static INetworkConnection OnFailedConnection(
            this INetworkConnection networkConnection, Action action)
        {
            if (networkConnection.IsFailureConnection() && action != null)
            {
                action();
            }

            return networkConnection;
        }

        public static void OnBoth(
            this INetworkConnection networkConnection, Action action)
        {
            action?.Invoke();
        }
    }
}