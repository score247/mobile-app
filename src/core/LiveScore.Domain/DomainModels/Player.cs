namespace LiveScore.Domain.DomainModels
{
    public interface IPlayer : IEntity<int, string>
    {
    }

    public class Player : Entity<int, string>, IPlayer
    {
    }
}