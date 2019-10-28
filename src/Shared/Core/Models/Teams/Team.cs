using System.Collections.Generic;

namespace LiveScore.Core.Models.Teams
{
    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        string Flag { get; }

        bool IsHome { get; }

        string Formation { get; }

        string Abbreviation { get; }

        ITeamStatistic Statistic { get; set; }

        ICoach Coach { get; }

        IEnumerable<IPlayer> Players { get; }

        IEnumerable<IPlayer> Substitutions { get; }
    }
}