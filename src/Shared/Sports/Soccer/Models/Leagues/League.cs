using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject]
    public class League : ILeague
    {
        public League(
            string id,
            string name,
            int order,
            string categoryId,
            string countryName,
            string countryCode,
            bool isInternational)
        {
            Id = id;
            Name = name;
            Order = order;
            CategoryId = categoryId;
            CountryName = countryName;
            CountryCode = countryCode;
            IsInternational = isInternational;
        }

        [Key(0)]
        public string Id { get; private set; }

        [Key(1)]
        public string Name { get; private set; }

        [Key(2)]
        public int Order { get; private set; }

        [Key(3)]
        public string CategoryId { get; private set; }

        [Key(4)]
        public string CountryName { get; private set; }

        [Key(5)]
        public string CountryCode { get; private set; }

        [Key(6)]
        public bool IsInternational { get; private set; }
    }
}