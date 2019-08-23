namespace LiveScore.Core.Events
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.Models.Teams;
    using Prism.Events;

    /// <summary>
    /// TODO: Temporary Add all pubsub event to this file and no immmutable, please do not review here, 
    /// </summary>
    public class MatchTimePubSubEvent : PubSubEvent<MatchTimeEvent>
    {
    }

    
    public class TeamStatisticPubSubEvent : PubSubEvent<TeamStatisticSignalRMessage>
    {
    }

    public class TeamStatisticSignalRMessage
    {
        public TeamStatisticSignalRMessage(
            byte sportId,
            TeamStatisticEvent teamStatistic)
        {
            SportId = sportId;
            TeamStatistic = teamStatistic;
        }

        public byte SportId { get; set; }

        public TeamStatisticEvent TeamStatistic { get; set; }
    }

    public class TeamStatisticEvent
    {
        public string MatchId { get; set; }

        public bool IsHome { get; set; }

        public TeamStatistic TeamStatistic { get; set; }
    }

    public class MatchEventPubSubEvent : PubSubEvent<MatchTimelineEvent>
    { }

    public class MatchTimelineEvent
    {
        public byte SportId { get; set; }

        public MatchEvent MatchEvent { get; set; }
    }

    public class OddsComparisonPubSubEvent : PubSubEvent<OddsComparisonSignalRMessage>
    { }

    public class OddsComparisonSignalRMessage
    {
        public OddsComparisonSignalRMessage(byte sportId, MatchOddsComparisonMessage oddsComparison)
        {
            SportId = sportId;
            OddsComparison = oddsComparison;
        }

        public byte SportId { get; set; }

        public MatchOddsComparisonMessage OddsComparison { get; set; }
    }
}