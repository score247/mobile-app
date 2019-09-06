namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using Odds;

    public interface IMatchOddsMovement
    {
        string MatchId { get; }

        Bookmaker Bookmaker { get; }

        IEnumerable<IOddsMovement> OddsMovements { get; }
    }
}