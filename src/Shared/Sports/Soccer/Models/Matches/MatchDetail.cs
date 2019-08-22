namespace LiveScore.Soccer.Models.Matches
{
    using System.Collections.Generic;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using Newtonsoft.Json;

    public class MatchDetail : IMatchDetail
    {
        [JsonConverter(typeof(JsonConcreteTypeConverter<Match>))]
        public IMatch Match { get; private set; }

        public byte AggregateHomeScore { get; private set; }

        public byte AggregateAwayScore { get; private set; }

        public IEnumerable<TimelineEvent> TimelineEvents { get; private set; }

        public int Attendance { get; private set; }

        public Venue Venue { get; private set; }

        public string Referee { get; private set; }
    }
}