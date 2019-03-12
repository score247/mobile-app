namespace Common.Models
{
    using Common.Models.MatchInfo;
    using Newtonsoft.Json;

    public class Match
    {
        [JsonProperty("sport_event")]
        public MatchEvent Event { get; set; }

        [JsonProperty("sport_event_status")]
        public MatchStatus Status { get; set; }
    }
}