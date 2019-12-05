using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Venue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}