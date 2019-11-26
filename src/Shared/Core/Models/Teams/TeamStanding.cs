namespace LiveScore.Core.Models.Teams
{
    public interface ITeamStanding
    {
        string Id { get; }

        string Name { get; }

        int Rank { get; }
    }
}