namespace LiveScore.Soccer.DTOs.Teams
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;

    public class TeamDto : Entity<string, string>
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public bool IsHome { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; }

        public TeamStatisticDto Statistic { get; set; }

        public CoachDto Coach { get; set; }

        public string Formation { get; set; }

        public string Abbreviation { get; set; }

        public IEnumerable<PlayerDto> Substitutions { get; set; }
    }
}