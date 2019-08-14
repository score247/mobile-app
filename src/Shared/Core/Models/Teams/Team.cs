namespace LiveScore.Core.Models.Teams
{
    using System.Collections.Generic;

    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        string Flag { get; }

        bool IsHome { get; }

        //"formation": "4-2-3-1",
        string Formation { get; }

        string Abbreviation { get; }

        ITeamStatistic Statistic { get; }

        ICoach Coach { get; }

        IEnumerable<IPlayer> Players { get; }

        IEnumerable<IPlayer> Substitutions { get; }
    }
}