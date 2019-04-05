namespace LiveScore.Domain.DomainModels
{
    using System.Collections.Generic;

    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        bool IsHome { get; }

        IEnumerable<Player> Players { get; }
    }

    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public bool IsHome { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}