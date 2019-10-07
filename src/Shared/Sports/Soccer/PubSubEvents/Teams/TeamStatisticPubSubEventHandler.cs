using LiveScore.Core.PubSubEvents.Teams;
using Newtonsoft.Json;
using Prism.Events;

namespace LiveScore.Soccer.PubSubEvents.Teams
{
    public class TeamStatisticPubSubEventHandler 
        : BasePubSubEventHandler<TeamStatisticsMessage, TeamStatisticPubSubEvent>
    {
        public TeamStatisticPubSubEventHandler(IEventAggregator eventAggregator)
             : base(eventAggregator)
        {
        }

        public override string HubMethod => "TeamStatistic";

        protected override void Publish(TeamStatisticsMessage message, TeamStatisticPubSubEvent pubSubEvent)
        {
            pubSubEvent.Publish(message);
        }
    }
}