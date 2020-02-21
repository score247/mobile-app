using System.Collections.Generic;
using LiveScore.Core.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TeamProfile : ITeamProfile
    {
        [SerializationConstructor]
        public TeamProfile(
              string id,
              string name,
              string country,
              string countryCode,
              string abbreviation)
        {
            Id = id;
            Name = name;
            Country = country;
            CountryCode = countryCode;
            Abbreviation = abbreviation;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Country { get; private set; }

        public string CountryCode { get; private set; }

        public string Abbreviation { get; private set; }

        [IgnoreMember]
        public string LogoUrl { get; set; }
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class TeamProfileList
    {
        public IEnumerable<TeamProfile> Teams { get; set; }
    }
}