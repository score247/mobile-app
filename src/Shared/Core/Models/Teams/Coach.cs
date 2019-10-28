using MessagePack;

namespace LiveScore.Core.Models.Teams
{
    public interface ICoach : IEntity<string, string>
    {
        string Nationality { get; }

        string CountryCode { get; }
    }

    [MessagePackObject]
    public class Coach : ICoach
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Nationality { get; set; }

        [Key(3)]
        public string CountryCode { get; set; }
    }
}