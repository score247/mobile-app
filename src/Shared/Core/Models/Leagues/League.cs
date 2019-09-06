namespace LiveScore.Core.Models.Leagues
{
    using System.Collections.Generic;
    using Models;
    using Teams;

    public interface ILeague : IEntity<string, string>
    {
        int Order { get; }

        string Flag { get; }

        ILeagueCategory Category { get; }

        IEnumerable<ILeagueRound> Rounds { get; }

        IEnumerable<ITeam> Teams { get; }
    }
}