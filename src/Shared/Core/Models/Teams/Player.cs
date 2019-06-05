﻿namespace LiveScore.Core.Models.Teams
{
    public interface IPlayer : IEntity<string, string>
    {
        string Type { get; }

        int JerseyNumber { get; }

        string Position { get; }

        int Order { get; }
    }

    public class Player : Entity<string, string>, IPlayer
    {
        public string Type { get; set; }

        public int JerseyNumber { get; set; }

        public string Position { get; set; }

        public int Order { get; set; }
    }
}