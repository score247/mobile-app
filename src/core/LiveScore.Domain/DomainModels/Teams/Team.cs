namespace LiveScore.Domain.DomainModels.Teams
{
    using System.Collections.Generic;

    public interface ITeam : IEntity<string, string>
    {
        string Country { get; }

        string CountryCode { get; }

        bool IsHome { get; }

        //"formation": "4-2-3-1",
        string Formation { get; }

        ITeamStatistic Statistic { get; }

        ICoach Coach { get; }

        IEnumerable<Player> Players { get; }

        IEnumerable<Player> Substitutions { get; }
    }

    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public bool IsHome { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public ITeamStatistic Statistic { get; set; }

        public ICoach Coach { get; set; }

        public string Formation { get; set; }

        public IEnumerable<Player> Substitutions { get; set; }
    }
}