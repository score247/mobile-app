using MessagePack;

namespace LiveScore.Core.Models.Teams
{
    using MessagePack;

    public interface IPlayer
    {
        string Type { get; }

        int JerseyNumber { get; }

        string Position { get; }

        int Order { get; }
    }

    [MessagePackObject]
    public class Player : IPlayer
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Type { get; set; }

        [Key(3)]
        public int JerseyNumber { get; set; }

        [Key(4)]
        public string Position { get; set; }

        [Key(5)]
        public int Order { get; set; }
    }
}