using System.Collections.Generic;

namespace LiveScore.Core.Models.Odds
{
    public interface IMatchOddsChangedMessage
    {
        string MatchId { get; }

        IEnumerable<IOddsEvent> OddsEvents { get; }
    }
}