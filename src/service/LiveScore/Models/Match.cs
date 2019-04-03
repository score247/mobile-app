namespace LiveScore.Models
{
    using Newtonsoft.Json;

    public class Match
    {
        [JsonProperty("sport_event")]
        public Event Event { get; set; }

        [JsonProperty("sport_event_status")]
        public MatchResult Status { get; set; }

        public TimeLine TimeLine { get; set; }

        [JsonProperty("sport_event_conditions")]
        public EventCondition EventCondition { get; set; }
    }
}