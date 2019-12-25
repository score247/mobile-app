using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class League : ILeague
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public League(
            string id,
            string name,
            int order,
            string categoryId,
            string countryName,
            string countryCode,
            bool isInternational,
            LeagueSeasonDates seasonDates,
            string roundGroup,
            string seasonId)
        {
            Id = id;
            Name = name;
            Order = order;
            CategoryId = categoryId;
            CountryName = countryName;
            CountryCode = countryCode;
            IsInternational = isInternational;
            RoundGroup = roundGroup;
            SeasonId = seasonId;
            SeasonDates = seasonDates;
        }

#pragma warning restore S107 // Methods should not have too many parameters

        public string Id { get; private set; }

        public string Name { get; private set; }

        public int Order { get; private set; }

        public string CategoryId { get; private set; }

        public string CountryName { get; private set; }

        public string CountryCode { get; private set; }

        public bool IsInternational { get; private set; }

        public LeagueSeasonDates SeasonDates { get; private set; }

        public string RoundGroup { get; private set; }

        public string SeasonId { get; private set; }

        public override bool Equals(object obj)
           => (obj is League actualObj) && Id == actualObj.Id && Name == actualObj.Name;

        public override int GetHashCode() => (Id?.GetHashCode() & Name?.GetHashCode()) ?? 0;
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueList
    {
        public IEnumerable<League> Leagues { get; set; }
    }
}