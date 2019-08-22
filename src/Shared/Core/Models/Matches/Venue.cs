namespace LiveScore.Core.Models.Matches
{
    public class Venue : Entity<string, string>
    {
        public int Capacity { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }
    }
}