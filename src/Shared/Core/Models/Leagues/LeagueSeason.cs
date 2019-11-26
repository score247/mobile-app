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

    [MessagePackObject]
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

        [Key(0)]
        public string Id { get; private set; }

        [Key(1)]
        public string Name { get; private set; }

        [Key(2)]
        public DateTimeOffset StartDate { get; private set; }

        [Key(3)]
        public DateTimeOffset EndDate { get; private set; }

        [Key(4)]
        public string Year { get; private set; }

        [Key(5)]
        public string LeagueId { get; private set; }
    }
}