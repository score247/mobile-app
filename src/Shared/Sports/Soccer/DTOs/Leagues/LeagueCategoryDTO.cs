namespace LiveScore.Soccer.DTOs.Leagues
{
    using LiveScore.Core.Models;

    public class LeagueCategoryDto : Entity<string, string>
    {
        public string CountryCode { get; set; }
    }
}