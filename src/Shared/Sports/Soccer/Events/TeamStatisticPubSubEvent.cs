namespace LiveScore.Soccer.Events
{
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Teams;
    using Prism.Events;

    public class TeamStatisticsMessage : ITeamStatisticsMessage
    {
        public const string HubMethod = "TeamStatistic";

        public TeamStatisticsMessage(
            byte sportId,
            string matchId,
            bool isHome,
            ITeamStatistic teamStatistic)
        {
            SportId = sportId;
            MatchId = matchId;
            IsHome = isHome;
            TeamStatistic = teamStatistic;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public bool IsHome { get; private set; }

        public ITeamStatistic TeamStatistic { get; private set; }

        public static void Publish(IEventAggregator eventAggregator, object data)
            => eventAggregator.GetEvent<TeamStatisticPubSubEvent>().Publish(data as TeamStatisticsMessage);
    }
}