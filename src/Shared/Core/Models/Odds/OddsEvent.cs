namespace LiveScore.Core.Models.Odds
{
    public interface IOddsEvent
    {
        int BetTypeId { get; }

        Bookmaker Bookmaker { get; }

        IOddsMovement OddsMovement { get; }
    }
}
