namespace LiveScore.Domain.DomainModels
{
    public interface ICategory
    {
        string Id { get; }

        string Name { get; }

        string CountryCode { get; }
    }

    public class Category : ICategory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }
    }
}
