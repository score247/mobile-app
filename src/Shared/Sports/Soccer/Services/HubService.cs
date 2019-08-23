namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Odds;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Events;

    public class SoccerHubService : IHubService
    {
        public const string MatchEvent = "MatchEvent";
        public const string OddsComparison = "OddsComparison";
        public const string OddsMovement = "OddsMovement";
        public const string PushMatchTime = "PushMatchTime";
        public const string TeamStatistic = "TeamStatistic";

        private readonly ISettingsService settingsService;
        private readonly IEventAggregator eventAggregator;
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private readonly ILoggingService logger;

        private HubConnection hubConnection;

        private static readonly IDictionary<string, (Type, Action<IEventAggregator, object>)> hubEvents =
            new Dictionary<string, (Type, Action<IEventAggregator, object>)>
            {
                { MatchEvent, (typeof(MatchTimelineEvent), PublishMatchEvent) },
                { OddsComparison, (typeof(OddsComparisonSignalRMessage), PublishOddsComparisonEvent) },
                { PushMatchTime, (typeof(MatchTimeEvent), PublishMatchTimeEvent) },
                { TeamStatistic, (typeof(TeamStatisticSignalRMessage), PublishTeamStatisticEvent) }
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
            catch(Exception ex)
            {
                await logger.LogErrorAsync(ex);
            }
        }

        private static void PublishMatchEvent(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<MatchEventPubSubEvent>().Publish(data as MatchTimelineEvent);

        private static void PublishOddsComparisonEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Publish(data as OddsComparisonSignalRMessage);

        private static void PublishTeamStatisticEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<TeamStatisticPubSubEvent>().Publish(data as TeamStatisticSignalRMessage);

        private static void PublishMatchTimeEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<MatchTimePubSubEvent>().Publish(data as MatchTimeEvent);

        public async Task Reconnect()
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync();
                }
            }
            catch(Exception ex)
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