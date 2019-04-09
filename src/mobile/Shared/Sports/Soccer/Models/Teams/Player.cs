namespace LiveScore.Soccer.Models.Teams
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;

    public class Player : Entity<int, string>, IPlayer
    {
        public string Type { get; set; }

        public int JerseyNumber { get; set; }

        public string Position { get; set; }

        public int Order { get; set; }
    }
}