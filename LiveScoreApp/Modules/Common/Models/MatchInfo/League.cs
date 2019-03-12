namespace Common.Models.MatchInfo
{
    using Newtonsoft.Json;

    public class League
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}