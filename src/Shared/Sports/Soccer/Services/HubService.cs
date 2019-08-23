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

    public class HubService : IHubService
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
                { MatchEvent, (typeof(MatchEvent), PublishMatchEvent) },
                { OddsComparison, (typeof(MatchOddsComparisonMessage), PublishOddsComparisonEvent) },
                { PushMatchTime, (typeof(MatchTimeEvent), PublishMatchTimeEvent) },
                { TeamStatistic, (typeof(TeamStatisticEvent), PublishTeamStatisticEvent) }
            };

        public byte SportId => SportType.Soccer.Value;

        public HubService(
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
            hubConnection = hubConnectionBuilder.WithUrl($"{settingsService.HubEndpoint}Soccer/MatchEventHub").Build();

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

        private static void PublishMatchEvent(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<MatchEventPubSubEvent>().Publish(data as MatchTimelineEvent);

        private static void PublishOddsComparisonEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Publish(data as MatchOddsComparisonMessage);

        private static void PublishTeamStatisticEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<TeamStatisticPubSubEvent>().Publish(data as TeamStatisticEvent);

        private static void PublishMatchTimeEvent(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<MatchTimePubSubEvent>().Publish(data as MatchTimeEvent);

        public async Task Reconnect()
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            await logger.LogErrorAsync(arg);
            await hubConnection.StartAsync();
        }
    }
}