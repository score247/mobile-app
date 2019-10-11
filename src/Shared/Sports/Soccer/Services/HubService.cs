using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly string hubEndpoint;

        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;
        private readonly INetworkConnection networkConnectionManager;

        private HubConnection hubConnection;

        public SoccerHubService(
            IHubConnectionBuilder hubConnectionBuilder,
            string hubEndpoint,
            ILoggingService logger,
            IEventAggregator eventAggregator,
            INetworkConnection networkConnectionManager)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
            this.hubEndpoint = hubEndpoint;
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.networkConnectionManager = networkConnectionManager;
        }

        public async Task Start()
        {
            try
            {
                await logger.LogInfoAsync($"{DateTime.Now} HubService start").ConfigureAwait(false);

                hubConnection = hubConnectionBuilder
                    .WithUrl($"{hubEndpoint}/soccerhub")
                    .Build();

                var handlers = BuildHandlers();

                foreach (var handler in handlers)
                {
                    hubConnection.On(
                        handler.HubMethod,
                        (Action<string>)((jsonString) =>
                        {
                            try
                            {
                                logger.LogInfoAsync($"HubService receiving {jsonString}");

                                handler.Handle(jsonString);
                            }
                            catch (Exception ex)
                            {
                                logger.LogExceptionAsync(ex);
                            }
                        }));
                }

                hubConnection.Closed += HubConnection_Closed;

                await hubConnection.StartAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await logger.LogExceptionAsync(ex).ConfigureAwait(false);
            }
        }

        private IEnumerable<IPubSubEventHandler> BuildHandlers()
            => new List<IPubSubEventHandler>
            {
                new MatchEventPubSubEventHandler(eventAggregator),
                new LiveMatchPubSubEventHandler(eventAggregator),
                new OddsComparisonPubSubEventHandler(eventAggregator),
                new OddsMovementPubSubEventHandler(eventAggregator),
                new TeamStatisticPubSubEventHandler(eventAggregator)
            };

        private async Task HubConnection_Closed(Exception arg)
        {
            if (networkConnectionManager.IsFailureConnection())
            {
                return;
            }

            var ex = new InvalidOperationException($"{DateTime.Now} HubConnection_Closed {arg.Message}", arg);

            await logger.LogInfoAsync($"HubConnection_Closed {ex}").ConfigureAwait(false);

            await ReConnect().ConfigureAwait(false);
        }

        public async Task ReConnect(byte retryTimes = 5)
        {
            var retryCount = 0;

            while (retryCount < retryTimes
                && hubConnection.State == HubConnectionState.Disconnected)
            {
                retryCount++;

                try
                {
                    await StopCurrentConnection().ConfigureAwait(false);
                    await Task.Delay(NumOfDelayMillisecondsBeforeReConnect).ConfigureAwait(false);
                    await hubConnection.StartAsync().ConfigureAwait(false);
                    await logger.LogInfoAsync($"Reconnect {retryCount} times, at {DateTime.Now}").ConfigureAwait(false);
                }
                catch (Exception startException)
                {
                    await logger
                        .LogExceptionAsync($"Reconnect Failed {retryCount} times, at {DateTime.Now}", startException)
                        .ConfigureAwait(false);
                }
            }
        }

        private async Task StopCurrentConnection()
        {
            try
            {
                await hubConnection.StopAsync().ConfigureAwait(false);
            }
            catch (Exception disposeException)
            {
                await logger.LogExceptionAsync(disposeException).ConfigureAwait(false);
            }
        }
    }
}