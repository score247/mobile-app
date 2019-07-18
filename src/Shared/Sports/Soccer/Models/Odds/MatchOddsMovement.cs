namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;

    public class MatchOddsMovement : IMatchOddsMovement
    {
        public string MatchId { get; set; }

        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<OddsMovement> OddsMovements{ get; set; }
    }
}
