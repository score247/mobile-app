namespace LiveScore.Core.Models.Teams
{
    public interface ITeamStatistic
    {
        byte Possession { get; }

        byte Fouls { get; }

        byte Injuries { get; }
    }
}