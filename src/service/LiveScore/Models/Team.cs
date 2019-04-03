namespace LiveScore.Models
{
    using Newtonsoft.Json;

    public class Team
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }
    }
}