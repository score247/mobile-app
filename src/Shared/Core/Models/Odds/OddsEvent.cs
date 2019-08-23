namespace LiveScore.Core.Models.Odds
{
    public interface IOddsMovementEvent
    {
        int BetTypeId { get; }

        Bookmaker Bookmaker { get; }

        IOddsMovement OddsMovement { get; }
    }
}
