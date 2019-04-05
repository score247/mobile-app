namespace LiveScore.Domain.Models
{
    public interface ISport : IEntity<string, string>
    {
    }

    public class Sport : Entity<string, string>, ISport
    {
    }
}