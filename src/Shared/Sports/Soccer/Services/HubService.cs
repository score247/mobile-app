using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Odds;
using LiveScore.Soccer.PubSubEvents.Matches;
using LiveScore.Soccer.PubSubEvents.Odds;
using LiveScore.Soccer.PubSubEvents.Teams;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
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
        private readonly INetworkConnectionManager networkConnectionManager;

        private HubConnection hubConnection;

        private static readonly IDictionary<string, (Type, Action<IEventAggregator, object>)> hubEvents
            = new Dictionary<string, (Type, Action<IEventAggregator, object>)>
            {
                { MatchEventMessage.HubMethod, (typeof(MatchEventMessage), MatchEventMessage.Publish) },
                { OddsComparisonMessage.HubMethod, (typeof(OddsComparisonMessage), OddsComparisonMessage.Publish) },
                { OddsMovementMessage.HubMethod, (typeof(OddsMovementMessage), OddsMovementMessage.Publish) },
                { TeamStatisticsMessage.HubMethod, (typeof(TeamStatisticsMessage), TeamStatisticsMessage.Publish) },
                { LiveMatchMessage.HubMethod, (typeof(LiveMatchMessage), LiveMatchMessage.Publish) }
            };

        public SoccerHubService(
            IHubConnectionBuilder hubConnectionBuilder,
            string hubEndpoint,
            ILoggingService logger,
            IEventAggregator eventAggregator,
            INetworkConnectionManager networkConnectionManager)
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

                foreach (var hubEvent in hubEvents)
                {
                    hubConnection.On(
                        hubEvent.Key,
                        (Action<string>)((jsonString) =>
                        {
                            try
                            {
                                logger.LogInfoAsync($"HubService receiving {jsonString}");

                                var data = JsonConvert.DeserializeObject(jsonString, hubEvent.Value.Item1);

                                if (data != null)
                                {
                                    hubEvent.Value.Item2(eventAggregator, data);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogErrorAsync(ex);
                            }
                        }));
                }

                hubConnection.Closed += HubConnection_Closed;

                await hubConnection.StartAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync(ex).ConfigureAwait(false);
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            if (networkConnectionManager.IsConnectionNotOK())
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
                        .LogErrorAsync($"Reconnect Failed {retryCount} times, at {DateTime.Now}", startException)
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
                await logger.LogErrorAsync(disposeException).ConfigureAwait(false);
            }
        }
    }
}