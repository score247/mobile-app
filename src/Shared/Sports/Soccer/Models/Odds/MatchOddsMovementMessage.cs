namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class MatchOddsMovementMessage : IOddsMovementMessage
    {
        public string MatchId { get; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<IOddsMovementEvent>>))]
        public IEnumerable<IOddsMovementEvent> OddsEvents { get; }
    }
}
