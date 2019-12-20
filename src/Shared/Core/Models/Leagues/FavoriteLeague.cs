namespace LiveScore.Core.Models.Leagues
{
    public class FavoriteLeague
    {
        public FavoriteLeague(string id, string groupName, string countryCode)
        {
            Id = id;
            GroupName = groupName;
            CountryCode = countryCode;
        }

        public string Id { get; }

        public string GroupName { get; }

        public string CountryCode { get; }
    }
}
