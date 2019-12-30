using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Services;
using LiveScore.Core.Services;
using LiveScore.Soccer.PubSubEvents;
using LiveScore.Soccer.PubSubEvents.Matches;
using LiveScore.Soccer.PubSubEvents.Odds;
using LiveScore.Soccer.PubSubEvents.Teams;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class SoccerHubService : IHubService
    {
        private const int NumOfDelayMillisecondsBeforeReConnect = 3_000;

        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;
        private readonly INetworkConnection networkConnectionManager;
        private readonly IConfiguration configuration;

        private HubConnection hubConnection;
        private bool isConnecting;

        public SoccerHubService(
            IHubConnectionBuilder hubConnectionBuilder,
            IConfiguration configuration,
            ILoggingService logger,
            IEventAggregator eventAggregator,
            INetworkConnection networkConnectionManager)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
            this.configuration = configuration;
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.networkConnectionManager = networkConnectionManager;
        }

        public async Task Start()
        {
            try
            {
                hubConnection = hubConnectionBuilder
                    .WithUrl($"{configuration.SignalRHubEndPoint}/soccerhub")
                    .Build();

                RegisterHubEvents();
                await ConnectWithRetry();
                SubscribeReConnectEvent();
            }
            catch (Exception ex)
            {
                await logger.LogExceptionAsync(ex);
            }
        }

        private void RegisterHubEvents()
        {
            var handlers = BuildHubEventHandlers();

            foreach (var handler in handlers)
            {
                hubConnection.On(
                    handler.HubMethod,
                    (Action<string>)((jsonString) =>
                    {
                        try
                        {
                            handler.Handle(jsonString);
                        }
                        catch (Exception ex)
                        {
                            logger.LogExceptionAsync(ex);
                        }
                    }));
            }

            hubConnection.Closed += HubConnection_Closed;
        }

        private void SubscribeReConnectEvent()
            => eventAggregator.GetEvent<ConnectionChangePubSubEvent>()
                            .Subscribe(async (isConnected) =>
                            {
                                if (isConnected)
                                {
                                    await ConnectWithRetry();
                                }
                            });

        private IEnumerable<IPubSubEventHandler> BuildHubEventHandlers()
        {
            return new List<IPubSubEventHandler>
            {
                new MatchEventPubSubEventHandler(eventAggregator),
                new LiveMatchPubSubEventHandler(eventAggregator),
                new OddsComparisonPubSubEventHandler(eventAggregator),
                new OddsMovementPubSubEventHandler(eventAggregator),
                new TeamStatisticPubSubEventHandler(eventAggregator),
                new MatchEventRemovedPubSubEventHandler(eventAggregator),
            };
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            if (networkConnectionManager.IsFailureConnection())
            {
                return;
            }

            var ex = new InvalidOperationException($"{DateTime.Now} HubConnection_Closed {arg?.Message}", arg);
            await logger.TrackEventAsync("SoccerHubService", $"HubConnection_Closed {ex}");
            await ConnectWithRetry();
        }

        public async Task ConnectWithRetry(byte retryTimes = 5)
        {
            var retryCount = 0;

            while (retryCount < retryTimes
                && hubConnection.State == HubConnectionState.Disconnected
                && !isConnecting)
            {
                retryCount++;
                isConnecting = true;

                try
                {
                    await hubConnection.StartAsync();
                    await Task.Delay(NumOfDelayMillisecondsBeforeReConnect);
                }
                catch (Exception exception)
                {
                    await logger
                        .LogExceptionAsync(exception, $"Connect Failed {retryCount} times, at {DateTime.Now}");
                }

                isConnecting = false;
            }
        }
    }
}