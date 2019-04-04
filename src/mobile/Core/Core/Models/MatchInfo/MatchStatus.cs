namespace Core.Models.MatchInfo
{
    using Newtonsoft.Json;

    public class MatchStatus
    {
        [JsonProperty("match_status")]
        public string Status { get; set; }

        [JsonProperty("home_score")]
        public int HomeScore { get; set; }

        [JsonProperty("away_score")]
        public int AwayScore { get; set; }
    }
}