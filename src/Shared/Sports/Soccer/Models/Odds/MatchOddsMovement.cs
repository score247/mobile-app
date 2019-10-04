using System.Collections.Generic;
using LiveScore.Core.Models.Odds;
using MessagePack;

namespace LiveScore.Soccer.Models.Odds
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchOddsMovement
    {
        public string MatchId { get; set; }

        public Bookmaker Bookmaker { get; set; }

        public IEnumerable<OddsMovement> OddsMovements { get; set; }
    }
}