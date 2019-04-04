namespace LiveScore.Domain.DomainModels
{
    using LiveScore.Domain.DomainModels;

    public interface ILeague
    {
        string Id { get; }

        string Name { get; }

        ICategory Category { get; }
    }

    public class League : ILeague
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICategory Category { get; set; }

        public bool Equals(League leagueA, League leagueB)
            => leagueA.Id == leagueB.Id;

        public int GetHashCode(League league)
            => league.Id.GetHashCode();
    }
}