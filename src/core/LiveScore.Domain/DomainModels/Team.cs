using System.Collections.Generic;

namespace LiveScore.Domain.DomainModels
{
    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        string Qualifier { get; }

        IEnumerable<Player> Players { get; }
    }

    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Qualifier { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}