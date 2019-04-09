namespace LiveScore.Soccer.DTOs.Matches
{
    using LiveScore.Core.Models;

    public class VenueDTO : Entity<string, string>
    {
        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}