namespace LiveScore.Domain.DomainModels
{
    public interface ISport : IEntity<string, string>
    {
    }

    public class Sport : Entity<string, string>, ISport
    {
    }
}