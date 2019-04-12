namespace LiveScore.Soccer.DTOs.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Soccer.DTOs.Teams;

    public class LeagueDTO : Entity<string, string>
    {
        public int Order { get; set; }

        public string Flag { get; set; }

        public LeagueCategoryDTO Category { get; set; }

        public IEnumerable<LeagueRoundDTO> Rounds { get; set; }

        public IEnumerable<TeamDTO> Teams { get; set; }
    }
}