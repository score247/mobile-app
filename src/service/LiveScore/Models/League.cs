namespace LiveScore.Models
{
    using Newtonsoft.Json;

    public class League
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        public bool Equals(League leagueA, League leagueB)
            => leagueA.Id == leagueB.Id;

        public int GetHashCode(League league)
            => league.Id.GetHashCode();
    }

    public class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}