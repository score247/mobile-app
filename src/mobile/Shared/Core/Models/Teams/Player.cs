namespace LiveScore.Core.Models.Teams
{
    public interface IPlayer : IEntity<int, string>
    {
        string Type { get; }

        int JerseyNumber { get; }

        string Position { get; }

        int Order { get; }
    }
}