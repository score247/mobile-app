using System.Collections.Generic;
using LiveScore.Core.Models.Odds;
using MessagePack;

namespace LiveScore.Soccer.Models.Odds
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchOddsMovement
    {
        public MatchOddsMovement()
        { }

        [SerializationConstructor]
        public MatchOddsMovement(string matchId, Bookmaker bookmaker, IEnumerable<OddsMovement> oddsMovements)
        {
            MatchId = matchId;
            Bookmaker = bookmaker;
            OddsMovements = oddsMovements;
        }

        public string MatchId { get; }

        public Bookmaker Bookmaker { get; }

        public IEnumerable<OddsMovement> OddsMovements { get; }
    }
}