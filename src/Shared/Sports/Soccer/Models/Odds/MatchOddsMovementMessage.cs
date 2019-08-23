namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class MatchOddsMovementMessage : IOddsMovementMessage
    {
        public string MatchId { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<OddsMovementEvent>>))]
        public IEnumerable<IOddsMovementEvent> OddsEvents { get; set; }
    }
}
