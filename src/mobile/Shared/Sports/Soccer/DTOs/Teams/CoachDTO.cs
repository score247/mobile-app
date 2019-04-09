namespace LiveScore.Soccer.DTOs.Teams
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;

    public class CoachDTO : Entity<int, string>
    {
        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}