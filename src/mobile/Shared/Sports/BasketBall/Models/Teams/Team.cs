namespace LiveScore.Basketball.Models.Teams
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;

    public class Team : Entity<string, string>, ITeam
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public bool IsHome { get; set; }

        public IEnumerable<IPlayer> Players { get; set; }

        public ITeamStatistic Statistic { get; set; }

        public ICoach Coach { get; set; }

        public string Formation { get; set; }

        public string Abbreviation { get; set; }

        public IEnumerable<IPlayer> Substitutions { get; set; }
    }
}