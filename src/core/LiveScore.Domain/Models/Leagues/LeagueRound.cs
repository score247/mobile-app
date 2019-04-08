namespace LiveScore.Domain.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Domain.Enumerations;
    using LiveScore.Domain.Models.Matches;

    public interface ILeagueRound
    {
        LeagueRoundTypes Type { get; }

        string Name { get; }

        int Number { get; }

        IEnumerable<Match> Matches { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public IEnumerable<Match> Matches { get; set; }
    }
}