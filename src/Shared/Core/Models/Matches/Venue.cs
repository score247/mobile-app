using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    [MessagePackObject]
    public class Venue
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public int Capacity { get; set; }

        [Key(3)]
        public string CityName { get; set; }

        [Key(4)]
        public string CountryName { get; set; }
    }
}