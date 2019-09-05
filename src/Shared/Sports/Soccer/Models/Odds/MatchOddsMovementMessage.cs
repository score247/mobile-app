namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.PubSubEvents.Odds;
    using Newtonsoft.Json;
    using Prism.Events;

    public class OddsMovementMessage : IOddsMovementMessage
    {
        public const string HubMethod = "OddsMovement";

        public OddsMovementMessage(
           byte sportId,
           string matchId,
           IEnumerable<IOddsMovementEvent> oddsEvents)
        {
            SportId = sportId;
            MatchId = matchId;
            OddsEvents = oddsEvents;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<OddsMovementEvent>>))]
        public IEnumerable<IOddsMovementEvent> OddsEvents { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<OddsMovementPubSubEvent>().Publish(data as OddsMovementMessage);
    }
}