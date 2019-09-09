using LiveScore.Core;

namespace LiveScore.Soccer.Services
{
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

    public class SoccerHubService : IHubService
    {
        private readonly ISettings settings;
        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;

        private HubConnection hubConnection;

        private static readonly IDictionary<string, (Type, Action<IEventAggregator, object>)> hubEvents
            = new Dictionary<string, (Type, Action<IEventAggregator, object>)>
            {
                { MatchEventMessage.HubMethod, (typeof(MatchEventMessage), MatchEventMessage.Publish) },
                { OddsComparisonMessage.HubMethod, (typeof(OddsComparisonMessage), OddsComparisonMessage.Publish) },
                { OddsMovementMessage.HubMethod, (typeof(OddsMovementMessage), OddsMovementMessage.Publish) },
                { TeamStatisticsMessage.HubMethod, (typeof(TeamStatisticsMessage), TeamStatisticsMessage.Publish) }
            };

        public SoccerHubService(
            IHubConnectionBuilder hubConnectionBuilder,
            ILoggingService logger,
            ISettings settings,
            IEventAggregator eventAggregator)
        {
            this.settings = settings;
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.hubConnectionBuilder = hubConnectionBuilder;
        }

        public async Task Start()
        {
            try
            {
                hubConnection = hubConnectionBuilder.WithUrl($"{settings.HubEndpoint}/soccerhub").Build();

                foreach (var hubEvent in hubEvents)
                {
                    hubConnection.On(
                        hubEvent.Key,
                        (Action<string>)((jsonString) =>
                        {
                            var data = JsonConvert.DeserializeObject(jsonString, hubEvent.Value.Item1);

                            if (data != null)
                            {
                                hubEvent.Value.Item2(eventAggregator, data);
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
                await logger.LogErrorAsync(ex).ConfigureAwait(false);
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            await logger.LogErrorAsync(arg).ConfigureAwait(false);
            await hubConnection.StartAsync().ConfigureAwait(false);
        }
    }
}