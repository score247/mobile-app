namespace League.Models
{
    using System.Collections.Generic;
    using Core.Models.MatchInfo;
    using Newtonsoft.Json;

    public class LeagueInfo
    {
        [JsonProperty(PropertyName = "tournaments")]
        public IList<League> Leagues { get; set; }
    }
}
