namespace LiveScore.Soccer.DTOs.Leagues
{
    using LiveScore.Core.Models;

    public class LeagueCategoryDTO : Entity<string, string>
    {
        public string CountryCode { get; set; }
    }
}