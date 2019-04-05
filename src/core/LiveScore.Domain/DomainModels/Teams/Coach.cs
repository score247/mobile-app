namespace LiveScore.Domain.DomainModels.Teams
{
    public interface ICoach : IEntity<int, string>
    {
        string Nationality { get; }

        string CountryCode { get; }
    }

    public class Coach : Entity<int, string>, ICoach
    {
        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}