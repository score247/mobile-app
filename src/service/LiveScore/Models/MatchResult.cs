namespace LiveScore.Models
{
    using Newtonsoft.Json;

    public class MatchResult
    {
        [JsonProperty("match_status")]
        public string Status { get; set; }

        [JsonProperty("home_score")]
        public int HomeScore { get; set; }

        [JsonProperty("away_score")]
        public int AwayScore { get; set; }
    }
}