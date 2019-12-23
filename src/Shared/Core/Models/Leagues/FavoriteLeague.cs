namespace LiveScore.Core.Models.Leagues
{
    public class FavoriteLeague
    {
        public FavoriteLeague(string id, string leagueGroupName, string countryFlag, string leagueSeasonId, string leagueRoundGroup, int order)
        {
            Id = id;
            LeagueGroupName = leagueGroupName;
            CountryFlag = countryFlag;
            LeagueSeasonId = leagueSeasonId;
            LeagueRoundGroup = leagueRoundGroup;
            Order = order;
        }

        public string Id { get; }

        public string LeagueGroupName { get; }

        public string CountryFlag { get; }

        public string LeagueSeasonId { get; }

        public string LeagueRoundGroup { get; }

        public int Order { get; }
    }
}
