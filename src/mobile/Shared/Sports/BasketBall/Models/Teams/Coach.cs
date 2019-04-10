namespace LiveScore.Basketball.Models.Teams
{
    using LiveScore.Core.Models;
    using LiveScore.Core.Models.Teams;

    public class Coach : Entity<int, string>, ICoach
    {
        public string Nationality { get; set; }

        public string CountryCode { get; set; }
    }
}