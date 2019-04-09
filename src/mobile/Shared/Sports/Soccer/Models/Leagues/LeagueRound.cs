namespace LiveScore.Soccer.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;

    public class LeagueRound : ILeagueRound
    {
        public LeagueRoundTypes Type { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public IEnumerable<IMatch> Matches { get; set; }
    }
}