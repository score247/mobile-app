namespace LiveScore.Domain.DomainModels
{
    public interface IReferee : IEntity<int, string>
    {
    }

    public class Referee : Entity<int, string>, IReferee
    {
    }
}