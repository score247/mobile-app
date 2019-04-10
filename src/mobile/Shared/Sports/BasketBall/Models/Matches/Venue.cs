namespace LiveScore.Basketball.Models.Matches
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Matches;

    public class Venue : Entity<string, string>, IVenue
    {
        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}