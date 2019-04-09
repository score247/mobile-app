namespace LiveScore.Core.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public interface ILeagueRound
    {
        LeagueRoundTypes Type { get; }

        string Name { get; }

        int Number { get; }

        IEnumerable<IMatch> Matches { get; }
    }
}