namespace LiveScore.Domain.DomainModels.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Domain.DomainModels.Teams;

    public interface ILeague : IEntity<string, string>
    {
        int Order { get; }

        ILeagueCategory Category { get; }

        IEnumerable<ILeagueRound> Rounds { get; }

        IEnumerable<Team> Teams { get; }
    }

    public class League : Entity<string, string>, ILeague
    {
        public int Order { get; set; }

        public ILeagueCategory Category { get; set; }

        public IEnumerable<ILeagueRound> Rounds { get; set; }

        public IEnumerable<Team> Teams { get; set; }
    }
}