namespace LiveScore.Core.Models
{
    public interface IEntity<out TId, out TName>
    {
        TId Id { get; }

        TName Name { get; }
    }

    public class Entity<TId, TName>
    {
        public TId Id { get; set; }

        public TName Name { get; set; }
    }
}