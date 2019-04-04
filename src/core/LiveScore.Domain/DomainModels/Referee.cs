namespace LiveScore.Domain.DomainModels
{
    public interface IReferee
    {
        int Id { get; }

        string Name { get; }
    }

    public class Referee : IReferee
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
