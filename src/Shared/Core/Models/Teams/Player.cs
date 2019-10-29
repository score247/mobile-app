namespace LiveScore.Core.Models.Teams
{
    public interface IPlayer
    {
        string Id { get; set; }

        string Name { get; set; }

        int JerseyNumber { get; }

        int Order { get; }
    }
}