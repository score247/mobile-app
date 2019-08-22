namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.Odds;

    public interface IMatchOddsMovement
    {
        string MatchId { get; }

        Bookmaker Bookmaker { get; }

        IEnumerable<IOddsMovement> OddsMovements { get; }
    }
}