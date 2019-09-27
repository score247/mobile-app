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
        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;
        private readonly string hubEndpoint;

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
            IEventAggregator eventAggregator)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
            this.hubEndpoint = hubEndpoint;
            this.logger = logger;
            this.eventAggregator = eventAggregator;
        }

        public async Task Start()
        {
            try
            {
                await logger.LogInfoAsync($"{DateTime.Now} HubService start").ConfigureAwait(false);

                hubConnection = hubConnectionBuilder.WithUrl($"{hubEndpoint}/soccerhub").Build();

                foreach (var hubEvent in hubEvents)
                {
                    hubConnection.On(
                        hubEvent.Key,
                        (Action<string>)((jsonString) =>
                        {
                            try
                            {
                                logger.LogInfo($"HubService receiving {jsonString}");

                                var data = JsonConvert.DeserializeObject(jsonString, hubEvent.Value.Item1);

                                if (data != null)
                                {
                                    hubEvent.Value.Item2(eventAggregator, data);
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex);
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

        public async Task Reconnect()
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync($"HubService Reconnect exception {ex.Message}", ex).ConfigureAwait(false);
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            var ex = new InvalidOperationException($"{DateTime.Now} HubConnection_Closed {arg.Message}", arg);

            await logger.LogErrorAsync($"HubConnection_Closed {arg.Message}", ex).ConfigureAwait(false);

            await hubConnection.StartAsync().ConfigureAwait(false);
        }
    }
}