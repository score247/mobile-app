namespace LiveScore.Models
{
    using Newtonsoft.Json;

    public class LeagueRound
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "cup_round_matches")]
        public int CupRoundMatches { get; set; }

        [JsonProperty(PropertyName = "cup_round_match_number")]
        public int CupRoundMatchNumber { get; set; }
    }
}