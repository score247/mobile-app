namespace LiveScore.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Models;
    using Newtonsoft.Json;

    public class DailyScheduleViewModel
    {
        private IEnumerable<DailyScheduleItem> dailyScheduleItems;

        [JsonProperty(PropertyName = "results")]
        public IEnumerable<Match> Matches { get; set; }

        [JsonProperty(PropertyName = "generated_at")]
        public DateTime GeneratedTime { get; set; }

        public IEnumerable<DailyScheduleItem> DailyScheduleItems
        {
            get
            {
                if(dailyScheduleItems == null)
                {
                    dailyScheduleItems = BuildDailyScheduleItems();
                }

                return dailyScheduleItems;
            }
        }

        private IEnumerable<DailyScheduleItem> BuildDailyScheduleItems()
        {
            if(Matches == null || !Matches.Any())
            {
                return Enumerable.Empty<DailyScheduleItem>();
            }

            return Matches
                .GroupBy(match => match.Event?.League)
                .Select(group => 
                    new DailyScheduleItem
                    {
                        League = group.Key,
                        Matches = group.ToList()
                    });
        }
    }

    public class DailyScheduleItem
    {
        public League League { get; set; }

        public IList<Match> Matches { get; set; }
    }
}