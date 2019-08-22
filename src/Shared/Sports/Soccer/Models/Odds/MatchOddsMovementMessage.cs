namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class MatchOddsChangedMessage : IMatchOddsChangedMessage
    {
        public string MatchId { get; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<IOddsEvent>>))]
        public IEnumerable<IOddsEvent> OddsEvents { get; }
    }
}