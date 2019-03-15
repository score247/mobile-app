namespace Common.Models.MatchInfo
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MatchEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("scheduled")]
        public DateTime EventDate { get; set; }

        [JsonProperty("tournament")]
        public League League { get; set; }

        [JsonProperty("competitors")]
        public IList<Team> Teams { get; set; }

        [JsonProperty("venue")]
        public Venue Venue { get; set; }

        public string ShortEventDate { get; set; }
    }
}