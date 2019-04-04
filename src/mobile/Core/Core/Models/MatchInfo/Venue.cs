namespace Core.Models.MatchInfo
{
    using Newtonsoft.Json;

    public class Venue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }
    }
}