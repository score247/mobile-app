using MessagePack;

namespace LiveScore.Core.Models.Teams
{
    public interface ICoach
    {
        string Id { get; set; }

        string Name { get; set; }

        string Nationality { get; }

        string CountryCode { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class Coach : ICoach
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}