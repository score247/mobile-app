namespace LiveScore.Soccer.Events
{
    using System.Collections.Generic;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Odds;
    using Prism.Events;

    public class OddsComparisonMessage : IOddsComparisonMessage
    {
        public const string HubMethod = "OddsComparison";

        public OddsComparisonMessage(
            byte sportId,
            string matchId,
            IEnumerable<IBetTypeOdds> betTypeOddsList)
        {
            SportId = sportId;
            MatchId = matchId;
            BetTypeOddsList = betTypeOddsList;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public IEnumerable<IBetTypeOdds> BetTypeOddsList { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Publish(data as OddsComparisonMessage);
    }
}