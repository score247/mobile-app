namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class MatchOddsComparisonMessage : IOddsComparisonMessage
    {
        public string MatchId { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<BetTypeOdds>>))]
        public IEnumerable<IBetTypeOdds> BetTypeOddsList { get; set; }
    }
}
