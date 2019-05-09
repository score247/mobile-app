namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using Newtonsoft.Json;

    public class MatchPayload
    {
        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchResult>))]
        public IMatchResult MatchResult { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<TimeLine>>))]
        public IEnumerable<ITimeLine> Timelines { get; set; }
    }
}
