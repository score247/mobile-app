using System.Collections.Generic;

namespace LiveScore.Core.Models.Odds
{
    public interface IOddsMovementMessage
    {
        string MatchId { get; }

        IEnumerable<IOddsMovementEvent> OddsEvents { get; }
    }
}
