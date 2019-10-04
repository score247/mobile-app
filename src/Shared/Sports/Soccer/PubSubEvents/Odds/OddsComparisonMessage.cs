using System.Collections.Generic;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.PubSubEvents.Odds;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Odds
{
    public class OddsComparisonMessage
    {
        public const string HubMethod = "OddsComparison";

        public OddsComparisonMessage(
            byte sportId,
            string matchId,
            IEnumerable<BetTypeOdds> betTypeOddsList)
        {
            SportId = sportId;
            MatchId = matchId;
            BetTypeOddsList = betTypeOddsList;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public IEnumerable<BetTypeOdds> BetTypeOddsList { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Publish(data as OddsComparisonMessage);
    }
}