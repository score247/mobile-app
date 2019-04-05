﻿namespace LiveScore.Core.Models.LeagueInfo
{
    using System.Collections.Generic;
    using LiveScore.Core.Models.MatchInfo;
    using Newtonsoft.Json;

    public class LeagueSchedule
    {
        [JsonProperty(PropertyName = "sport_events")]
        public IList<MatchEvent> SportEvents { get; set; }
    }
}
