namespace LiveScore.Models
{
    using Newtonsoft.Json;


    ////"venue": {
    ////        "id": "sr:venue:843",
    ////        "name": "Parc Des Princes",
    ////        "capacity": 45000,
    ////        "city_name": "Paris",
    ////        "country_name": "France",
    ////        "map_coordinates": "48.841389,2.253056",
    ////        "country_code": "FRA"
    ////    },
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