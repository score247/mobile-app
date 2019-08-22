namespace LiveScore.Soccer.Models.Odds
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;
    using Newtonsoft.Json;

    public class MatchOddsMovement : IMatchOddsMovement
    {
        public string MatchId { get; set; }

        public Bookmaker Bookmaker { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<OddsMovement>>))]
        public IEnumerable<IOddsMovement> OddsMovements { get; set; }
    }
}