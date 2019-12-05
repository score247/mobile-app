using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
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

        
        public string Id { get; private set; }

        
        public string Name { get; private set; }

        
        public int Order { get; private set; }

        
        public string CategoryId { get; private set; }

        
        public string CountryName { get; private set; }

        
        public string CountryCode { get; private set; }

        
        public bool IsInternational { get; private set; }
    }
}