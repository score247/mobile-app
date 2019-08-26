namespace LiveScore.Core.PubSubEvents.Teams
{
    using LiveScore.Core.Models.Teams;

    public interface ITeamStatisticsMessage
    {
        byte SportId { get; }

        string MatchId { get; }

        bool IsHome { get; }

        ITeamStatistic TeamStatistic { get; }
    }
}