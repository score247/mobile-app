﻿namespace League.Models
{
    using System.Collections.Generic;
    using Common.Models.MatchInfo;
    using Newtonsoft.Json;

    public class LeagueInfo
    {
        [JsonProperty(PropertyName = "tournaments")]
        public IList<League> Leagues { get; set; }
    }
}