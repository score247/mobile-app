namespace LiveScore.Soccer.Models.Odds
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class OddsEvent : IOddsEvent
    {
        public OddsEvent(
            int betTypeId,
            Bookmaker bookmaker,
            OddsMovement oddsMovement)
        {
            BetTypeId = betTypeId;
            Bookmaker = bookmaker;
            OddsMovement = oddsMovement;
        }

        public int BetTypeId { get; }

        public Bookmaker Bookmaker { get; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<OddsMovement>))]
        public IOddsMovement OddsMovement { get; }
    }
}