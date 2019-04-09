namespace LiveScore.Soccer.DTOs.Leagues
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;

    public class LeagueCategoryDTO : Entity<string, string>
    {
        public string CountryCode { get; set; }
    }
}