namespace LiveScore.Soccer.DTOs.Teams
{
    using LiveScore.Core.Models;

    public class CoachDTO : Entity<int, string>
    {
        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}