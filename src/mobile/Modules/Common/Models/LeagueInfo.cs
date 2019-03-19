using System;
namespace Common.Models
{
    using System;
    using System.Collections.Generic;
    using Common.Models.MatchInfo;
    using Newtonsoft.Json;

    public class LeagueInfo
    {
        [JsonProperty(PropertyName = "tournaments")]
        public IList<League> Leagues { get; set; }
    }
}
