namespace LiveScore.Domain.DomainModels.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Domain.DomainModels.Matches;

    public interface ILeagueRound
    {
        string Type { get; }

        string Name { get; }

        string Number { get; }

        IEnumerable<Match> Matches { get; }
    }

    public class LeagueRound : ILeagueRound
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public IEnumerable<Match> Matches { get; set; }
    }
}