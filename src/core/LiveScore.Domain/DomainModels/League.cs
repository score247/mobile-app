namespace LiveScore.Domain.DomainModels
{
    using System.Collections.Generic;

    public interface ILeague : IEntity<string, string>
    {
        ILeagueCategory Category { get; }

        IEnumerable<ILeagueRound> Rounds { get; }

        IEnumerable<Team> Teams { get; }
    }

    public class League : Entity<string, string>, ILeague
    {
        public ILeagueCategory Category { get; set; }

        public IEnumerable<ILeagueRound> Rounds { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public bool Equals(League leagueA, League leagueB)
            => leagueA.Id == leagueB.Id;

        public int GetHashCode(League league)
            => league.Id.GetHashCode();
    }
}