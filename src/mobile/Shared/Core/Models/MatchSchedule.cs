﻿namespace LiveScore.Core.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using LiveScore.Core.Models.MatchInfo;

    public class MatchSchedule
    {
        [JsonProperty(PropertyName = "results")]
        public IList<Match> Matches { get; set; }

        [JsonProperty(PropertyName = "generated_at")]
        public DateTime GeneratedTime { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }
    }
}