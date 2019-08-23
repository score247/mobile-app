namespace LiveScore.Core.Events
{
    using LiveScore.Core.Models.Teams;
    using Prism.Events;

    public class TeamStatisticPubSubEvent : PubSubEvent<ITeamStatisticsMessage>
    {
    }

    public interface ITeamStatisticsMessage
    {
        byte SportId { get; }

        string MatchId { get; }

        bool IsHome { get; }

        ITeamStatistic TeamStatistic { get; }
    }
}