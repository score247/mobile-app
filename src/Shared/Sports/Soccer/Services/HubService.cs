﻿namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Events;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Events;

    public class SoccerHubService : IHubService
    {
        private readonly ISettingsService settingsService;
        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;

        private HubConnection hubConnection;

        private static readonly IDictionary<string, (Type, Action<IEventAggregator, object>)> hubEvents
            = new Dictionary<string, (Type, Action<IEventAggregator, object>)>
            {
                { MatchEventMessage.HubMethod, (typeof(MatchEventMessage), MatchEventMessage.Publish) },
                { OddsComparisonMessage.HubMethod, (typeof(OddsComparisonMessage), OddsComparisonMessage.Publish) },
                { TeamStatisticsMessage.HubMethod, (typeof(TeamStatisticsMessage), TeamStatisticsMessage.Publish) }
            };

        public byte SportId => SportType.Soccer.Value;

        public SoccerHubService(
            IHubConnectionBuilder hubConnectionBuilder,
            ILoggingService logger,
            ISettingsService settingsService,
            IEventAggregator eventAggregator)
        {
            this.settingsService = settingsService;
            this.logger = logger;
            this.eventAggregator = eventAggregator;
            this.hubConnectionBuilder = hubConnectionBuilder;
        }

        public async Task Start()
        {
            try
            {
                hubConnection = hubConnectionBuilder.WithUrl($"{settingsService.HubEndpoint}/soccerhub").Build();

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

                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync(ex);
            }
        }

        public async Task Reconnect()
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync();
                }
            }
            catch (Exception ex)
            {
                await logger.LogErrorAsync(ex);
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            await logger.LogErrorAsync(arg);
            await hubConnection.StartAsync();
        }
    }
}