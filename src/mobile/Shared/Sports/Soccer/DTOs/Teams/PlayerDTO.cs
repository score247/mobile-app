namespace LiveScore.Soccer.DTOs.Teams
{
    using LiveScore.Core.Models;

    public class PlayerDTO : Entity<int, string>
    {
        public string Type { get; set; }

        public int JerseyNumber { get; set; }

        public string Position { get; set; }

        public int Order { get; set; }
    }
}