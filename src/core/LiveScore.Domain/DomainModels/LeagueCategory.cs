namespace LiveScore.Domain.DomainModels
{
    public interface ICategory : IEntity<string, string>
    {
        string CountryCode { get; }
    }

    public class Category : Entity<string, string>, ICategory
    {
        public string CountryCode { get; set; }
    }
}