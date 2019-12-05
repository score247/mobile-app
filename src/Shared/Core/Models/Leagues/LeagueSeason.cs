using System;
using MessagePack;

namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueSeason
    {
        string Id { get; }

        string Name { get; }

        DateTimeOffset StartDate { get; }

        DateTimeOffset EndDate { get; }

        string Year { get; }

        string LeagueId { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueSeason : ILeagueSeason
    {
        public LeagueSeason(
            string id,
            string name,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string year,
            string leagueId)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Year = year;
            LeagueId = leagueId;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset EndDate { get; private set; }

        public string Year { get; private set; }

        public string LeagueId { get; private set; }
    }
}