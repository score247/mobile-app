namespace LiveScore.Domain.DomainModels
{
    public interface IPlayer
    {
        int Id { get; }

        string Name { get; }
    }

    public class Player : IPlayer
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}