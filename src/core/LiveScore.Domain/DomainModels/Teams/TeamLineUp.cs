namespace LiveScore.Domain.DomainModels.Teams
{
    public interface ITeamLineUp : IEntity<string, string>
    {
        string Type { get; }

        int JerseyNumber { get; }

        string Position { get; }

        int Order { get; }
    }

    public class TeamLineUp : Entity<string, string>, ITeamLineUp
    {
        public string Type { get; set; }

        public int JerseyNumber { get; set; }

        public string Position { get; set; }

        public int Order { get; set; }
    }
}