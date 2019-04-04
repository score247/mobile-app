namespace Core.Models.LeagueInfo
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class LeagueInfo
    {
        [JsonProperty(PropertyName = "tournaments")]
        public IList<League> Leagues { get; set; }
    }
}
