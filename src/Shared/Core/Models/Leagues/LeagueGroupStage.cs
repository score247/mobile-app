namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueGroupStage
    {
        string LeagueId { get; }

        string LeagueSeasonId { get; }

        string GroupStageName { get; }

        bool HasStanding { get; }

        LeagueRound LeagueRound { get; }
    }
}