namespace LiveScore.DomainModels
{

    public interface IVenue
    {
        string Id { get; }

        string Name { get; }

        int Capacity { get; }

        string CityName { get; }

        string CountryName { get; }
    }

    public class Venue : IVenue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}