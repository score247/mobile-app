namespace LiveScore.Soccer.DTOs.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Soccer.DTOs.Matches;

    public class LeagueRoundDTO
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public IEnumerable<MatchDTO> Matches { get; set; }
    }
}