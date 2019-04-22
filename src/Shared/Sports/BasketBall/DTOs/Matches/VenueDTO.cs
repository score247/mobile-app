namespace LiveScore.Basketball.DTOs.Matches
{
    using LiveScore.Core.Models;

    public class VenueDto : Entity<string, string>
    {
        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}