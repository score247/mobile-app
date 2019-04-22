namespace LiveScore.Basketball.DTOs.Teams
{
    using LiveScore.Core.Models;

    public class PlayerDto : Entity<int, string>
    {
        public string Type { get; set; }

        public int JerseyNumber { get; set; }

        public string Position { get; set; }

        public int Order { get; set; }
    }
}