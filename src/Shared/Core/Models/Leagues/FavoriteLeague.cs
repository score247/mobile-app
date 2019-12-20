namespace LiveScore.Core.Models.Leagues
{
    public class FavoriteLeague
    {
        public FavoriteLeague(string id, string name, string countryFlag)
        {
            Id = id;
            Name = name;
            CountryFlag = countryFlag;
        }

        public string Id { get; }

        public string Name { get; }

        public string CountryFlag { get; }
    }
}
