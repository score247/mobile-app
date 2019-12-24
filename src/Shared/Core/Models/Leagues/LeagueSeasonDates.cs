using System;
using MessagePack;

namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueSeasonDates
    {
        DateTimeOffset StartDate { get; }

        DateTimeOffset EndDate { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueSeasonDates : ILeagueSeasonDates
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