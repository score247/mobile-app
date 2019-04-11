﻿namespace LiveScore.Core.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;

    public interface ILeague : IEntity<string, string>
    {
        int Order { get; }

        string Flag { get; }

        ILeagueCategory Category { get; }

        IEnumerable<ILeagueRound> Rounds { get; }

        IEnumerable<ITeam> Teams { get; }
    }
}