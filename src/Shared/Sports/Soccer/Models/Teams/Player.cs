using LiveScore.Core.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
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
    }
}