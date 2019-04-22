namespace LiveScore.Basketball.DTOs.Teams
{
    using LiveScore.Core.Models;

    public class CoachDto : Entity<int, string>
    {
        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}