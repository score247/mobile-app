using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueTable
    {
        LeagueTableType Type { get; }
    }
}