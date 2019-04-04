namespace LiveScore.Domain.DomainModels
{
    public interface IEntity<out TId, out TName>
    {
        TId Id { get; }

        TName Name { get; }
    }

    public abstract class Entity<TId, TName>
    {
        public TId Id { get; set; }

        public TName Name { get; set; }
    }
}