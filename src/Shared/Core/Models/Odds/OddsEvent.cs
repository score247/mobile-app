namespace LiveScore.Core.Models.Odds
{
    public interface IOddsMovementEvent
    {
        byte BetTypeId { get; }

        Bookmaker Bookmaker { get; }

        IOddsMovement OddsMovement { get; }
    }
}
