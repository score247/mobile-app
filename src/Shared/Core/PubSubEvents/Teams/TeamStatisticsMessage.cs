using LiveScore.Core.Models.Teams;

namespace LiveScore.Core.PubSubEvents.Teams
{
    public interface ITeamStatisticsMessage
    {
        byte SportId { get; }

        string MatchId { get; }

        bool IsHome { get; }

        ITeamStatistic TeamStatistic { get; }
    }
}