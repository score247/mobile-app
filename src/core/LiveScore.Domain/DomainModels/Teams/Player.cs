namespace LiveScore.Domain.DomainModels.Teams
{
    public interface IPlayer : IEntity<int, string>
    {
    }

    public class Player : Entity<int, string>, IPlayer
    {
    }
}