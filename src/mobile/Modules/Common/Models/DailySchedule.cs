namespace Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DailySchedule
    {
        [JsonProperty(PropertyName = "results")]
        public IList<Match> Matches { get; set; }

        [JsonProperty(PropertyName = "generated_at")]
        public DateTime GeneratedTime { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }
    }
}