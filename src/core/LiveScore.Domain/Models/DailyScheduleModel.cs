namespace LiveScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.DomainModels;

    public class DailyScheduleModel
    {
        private IEnumerable<DailyScheduleItem> dailyScheduleItems;

        public IEnumerable<IMatch> Matches { get; set; }

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
        public ILeague League { get; set; }

        public IList<IMatch> Matches { get; set; }
    }
}