namespace LiveScore.Soccer.Models.Leagues
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Leagues;

    public class LeagueCategory : Entity<string, string>, ILeagueCategory
    {
        public string CountryCode { get; set; }
    }
}