namespace LiveScore.BasketBall.Models.Leagues
{
    using System.Collections.Generic;
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;

    public class League : Entity<string, string>, ILeague
    {
        public int Order { get; set; }

        public string Flag { get; set; }

        public ILeagueCategory Category { get; set; }

        public IEnumerable<ILeagueRound> Rounds { get; set; }

        public IEnumerable<ITeam> Teams { get; set; }
    }
}