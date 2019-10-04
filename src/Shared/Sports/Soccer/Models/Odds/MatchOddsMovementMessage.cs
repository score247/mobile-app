using System.Collections.Generic;
using LiveScore.Core.PubSubEvents.Odds;
using Prism.Events;

namespace LiveScore.Soccer.Models.Odds
{
    public class OddsMovementMessage
    {
        public const string HubMethod = "OddsMovement";

        public OddsMovementMessage(
           byte sportId,
           string matchId,
           IEnumerable<OddsMovementEvent> oddsEvents)
        {
            SportId = sportId;
            MatchId = matchId;
            OddsEvents = oddsEvents;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public IEnumerable<OddsMovementEvent> OddsEvents { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
           => eventAggregator.GetEvent<OddsMovementPubSubEvent>().Publish(data as OddsMovementMessage);
    }
}