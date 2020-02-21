namespace LiveScore.Core.Models.Teams
{
    public interface ITeamProfile
    {
        string Id { get; }

        string Name { get; }

        string Country { get; }

        string CountryCode { get; }

        string Abbreviation { get; }

        string LogoUrl { get; set; }
    }
}