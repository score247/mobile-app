namespace LiveScore.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Domain.DomainModels;

    public class ScoresModel
    {
        private IEnumerable<MatchGroup> dailyScheduleItems;

        public IEnumerable<IMatch> Matches { get; set; }

        public DateTime GeneratedTime { get; set; }

        public IEnumerable<MatchGroup> DailyScheduleItems
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

        private IEnumerable<MatchGroup> BuildDailyScheduleItems()
        {
            if(Matches == null || !Matches.Any())
            {
                return Enumerable.Empty<MatchGroup>();
            }

            return Matches
                .GroupBy(match => match.Event?.League)
                .Select(group => 
                    new MatchGroup
                    {
                        League = group.Key,
                        Matches = group.ToList()
                    });
        }
    }

    public class MatchGroup
    {
        public ILeague League { get; set; }

        public IList<IMatch> Matches { get; set; }
    }
}