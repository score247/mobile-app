namespace LiveScore.Core.Models.Matches
{
    public interface IVenue : IEntity<string, string>
    {
        int Capacity { get; }

        string CityName { get; }

        string CountryName { get; }
    }
}