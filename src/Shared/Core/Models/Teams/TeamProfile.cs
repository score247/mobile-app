namespace LiveScore.Core.Models.Teams
{
    public interface ITeamProfile
    {
        string Id { get; }

        string Name { get; }

        string Country { get; }

        string CountryCode { get; }

        string Abbreviation { get; }

        string LogoName { get; }

        string LogoUrl { get; set; }

        bool IsFavorite { get; set; }
    }
}