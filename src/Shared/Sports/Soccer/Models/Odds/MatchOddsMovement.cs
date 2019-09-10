using MessagePack;

namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchOddsMovement
    {
        public string MatchId { get; set; }

        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<OddsMovement> OddsMovements { get; set; }
    }
}