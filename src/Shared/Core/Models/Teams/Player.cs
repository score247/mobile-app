using MessagePack;

namespace LiveScore.Core.Models.Teams
{
    public interface IPlayer
    {
        string Id { get; set; }

        string Name { get; set; }

        PlayerType Type { get; }

        int JerseyNumber { get; }

        Position Position { get; }

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
        public PlayerType Type { get; }

        [Key(3)]
        public int JerseyNumber { get; set; }

        [Key(4)]
        public Position Position { get; }

        [Key(5)]
        public int Order { get; set; }

        [IgnoreMember]
        public string JersryWithName => $"{JerseyNumber}. {Name}";
    }
}