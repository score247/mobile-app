namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueGroupStage
    {
        string LeagueId { get; }
        string LeagueSeasonId { get; }
        string GroupStageName { get; }
        LeagueRound LeagueRound { get; }
        string Language { get; }
    }
}