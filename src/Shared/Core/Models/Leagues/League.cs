namespace LiveScore.Core.Models.Leagues
{
    public interface ILeague
    {
        string Id { get; }

        string Name { get; }

        int Order { get; }

        string CategoryId { get; }

        string CountryName { get; }

        string CountryCode { get; }

        bool IsInternational { get; }

        string RoundGroup { get; }

        string SeasonId { get; }

        LeagueSeasonDates SeasonDates { get; }

        bool HasGroups { get; }
    }
}