namespace LiveScore.Domain.DomainModels
{
    public interface ITeam
    {
        string Id { get; }

        string Name { get; }

        string Country { get; }

        string CountryCode { get;  }

        string Qualifier { get; }
    }

    public class Team : ITeam
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Qualifier { get; set; }
    }
}