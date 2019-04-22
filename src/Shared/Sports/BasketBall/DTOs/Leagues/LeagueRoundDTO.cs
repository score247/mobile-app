namespace LiveScore.Basketball.DTOs.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Basketball.DTOs.Matches;

    public class LeagueRoundDto
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public IEnumerable<MatchDto> Matches { get; set; }
    }
}