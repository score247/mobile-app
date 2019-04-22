namespace LiveScore.Basketball.DTOs.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Basketball.DTOs.Teams;

    public class LeagueDto : Entity<string, string>
    {
        public int Order { get; set; }

        public string Flag { get; set; }

        public LeagueCategoryDto Category { get; set; }

        public IEnumerable<LeagueRoundDto> Rounds { get; set; }

        public IEnumerable<TeamDto> Teams { get; set; }
    }
}