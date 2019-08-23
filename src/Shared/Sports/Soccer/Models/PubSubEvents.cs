namespace LiveScore.Core.Events
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Odds;
    using Prism.Events;

    /// <summary>
    /// TODO: Temporary Add all pubsub event to this file
    /// </summary>
    public class MatchTimePubSubEvent : PubSubEvent<MatchTimeEvent>
    {
    }

    public class TeamStatisticPubSubEvent : PubSubEvent<TeamStatisticEvent>
    {
    }

    public class TeamStatisticEvent
    {
        public byte SportId { get; set; }

        public string MatchId { get; set; }

        public bool IsHome { get; set; }

        public ITeamStatistic TeamStatistic { get; set; }
    }

    public class MatchEventPubSubEvent : PubSubEvent<MatchTimelineEvent>
    { }

    public class MatchTimelineEvent
    {
        public byte SportId { get; }

        public IMatchEvent MatchEvent { get; }
    }

    public class OddsComparisonPubSubEvent : PubSubEvent<MatchOddsComparisonMessage>
    { }
}