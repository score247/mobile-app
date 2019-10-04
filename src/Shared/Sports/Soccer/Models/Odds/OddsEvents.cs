using LiveScore.Core.Models.Odds;

namespace LiveScore.Soccer.Models.Odds
{
    public class OddsMovementEvent
    {
        public OddsMovementEvent(
            byte betTypeId,
            Bookmaker bookmaker,
            OddsMovement oddsMovement)
        {
            BetTypeId = betTypeId;
            Bookmaker = bookmaker;
            OddsMovement = oddsMovement;
        }

        public byte BetTypeId { get; set; }

        public Bookmaker Bookmaker { get; set; }

        public OddsMovement OddsMovement { get; set; }
    }
}