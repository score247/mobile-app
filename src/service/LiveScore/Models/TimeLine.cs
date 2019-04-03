namespace LiveScore.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class TimeLine
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public DateTime Time { get; set; }

        [JsonProperty("match_time")]
        public int MatchTime { get; set; }

        [JsonProperty("match_time")]
        public string MatchClock { get; set; }

        public string Team { get; set; }

        public int Period { get; set; }

        [JsonProperty("period_type")]
        public string PeriodType { get; set; }

        [JsonProperty("home_score")]
        public int HomeScore { get; set; }

        [JsonProperty("away_score")]
        public int AwayScore { get; set; }

        [JsonProperty("goal_scorer")]
        public Player GoalScorer { get; set; }

        public IEnumerable<Commentary> Commentaries { get; set; }
    }

    public class Commentary
    {
        public string Text { get; set; }
    }
}