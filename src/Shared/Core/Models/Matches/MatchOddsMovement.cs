using System.Collections.Generic;
using LiveScore.Core.Models.Odds;

namespace LiveScore.Core.Models.Matches
{
    public interface IMatchOddsMovement
    {
        string MatchId { get; }

        Bookmaker Bookmaker { get; }

        IEnumerable<OddsMovement> OddsMovements { get; }
    }
}
