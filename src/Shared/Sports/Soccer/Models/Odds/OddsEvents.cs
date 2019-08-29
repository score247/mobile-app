﻿namespace LiveScore.Soccer.Models.Odds
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class OddsMovementEvent : IOddsMovementEvent
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

        [JsonConverter(typeof(JsonConcreteTypeConverter<OddsMovement>))]
        public IOddsMovement OddsMovement { get; set; }
    }
}