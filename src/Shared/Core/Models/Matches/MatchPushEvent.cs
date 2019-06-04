namespace LiveScore.Core.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using Newtonsoft.Json;

    public class MatchPushEvent
    {
        [JsonConverter(typeof(JsonConcreteTypeConverter<MatchResult>))]
        public IMatchResult MatchResult { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<IEnumerable<Timeline>>))]
        public IEnumerable<ITimeline> TimeLines { get; set; }
    }
}
