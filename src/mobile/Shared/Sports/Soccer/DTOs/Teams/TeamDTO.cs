namespace LiveScore.Soccer.DTOs.Teams
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;

    public class TeamDTO : Entity<string, string>
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public bool IsHome { get; set; }

        public IEnumerable<PlayerDTO> Players { get; set; }

        public TeamStatisticDTO Statistic { get; set; }

        public CoachDTO Coach { get; set; }

        public string Formation { get; set; }

        public string Abbreviation { get; set; }

        public IEnumerable<PlayerDTO> Substitutions { get; set; }
    }
}