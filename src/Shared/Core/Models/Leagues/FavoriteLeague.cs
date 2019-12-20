namespace LiveScore.Core.Models.Leagues
{
    public class FavoriteLeague
    {
        public FavoriteLeague(string id, string leagueGroupName, string countryFlag, string leagueSeasonId, string leagueRoundGroup)
        {
            Id = id;
            LeagueGroupName = leagueGroupName;
            CountryFlag = countryFlag;
            LeagueSeasonId = leagueSeasonId;
            LeagueRoundGroup = leagueRoundGroup;
        }

        public string Id { get; }

        public string LeagueGroupName { get; }

        public string CountryFlag { get; }

        public string LeagueSeasonId { get; }

        public string LeagueRoundGroup { get; }
    }
}
