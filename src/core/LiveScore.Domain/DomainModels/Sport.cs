namespace LiveScore.DomainModels
{
    public interface ISport
    {
        string Id { get; }

        string Name { get; }
    }

    public class Sport : ISport
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}