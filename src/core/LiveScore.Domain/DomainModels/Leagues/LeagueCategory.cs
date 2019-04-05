namespace LiveScore.Domain.DomainModels.Leagues
{
    public interface ILeagueCategory : IEntity<string, string>
    {
        string CountryCode { get; }
    }

    public class LeagueCategory : Entity<string, string>, ILeagueCategory
    {
        public string CountryCode { get; set; }
    }
}