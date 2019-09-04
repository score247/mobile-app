namespace LiveScore.Core.PubSubEvents.Odds
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;

    public interface IOddsMovementMessage
    {
        string MatchId { get; }

        IEnumerable<IOddsMovementEvent> OddsEvents { get; }
    }
}