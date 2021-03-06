// <auto-generated>
// Odds functions are disabled
// </auto-generated>
using System.Collections.Generic;

namespace LiveScore.Soccer.Models.Odds
{
    public class OddsMovementMessage
    {
        public OddsMovementMessage(
           byte sportId,
           string matchId,
           IEnumerable<OddsMovementEvent> oddsEvents)
        {
            SportId = sportId;
            MatchId = matchId;
            OddsEvents = oddsEvents;
        }

        public byte SportId { get; private set; }

        public string MatchId { get; private set; }

        public IEnumerable<OddsMovementEvent> OddsEvents { get; private set; }
    }
}