namespace LiveScore.Core.Models.Matches
{
    using MessagePack;

#pragma warning disable S109 // Magic numbers should not be used

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

#pragma warning restore S109 // Magic numbers should not be used
}