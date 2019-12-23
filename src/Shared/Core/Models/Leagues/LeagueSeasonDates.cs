using System;
using MessagePack;

namespace LiveScore.Core.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueSeasonDates
    {
        public LeagueSeasonDates(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset EndDate { get; private set; }
    }
}