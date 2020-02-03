using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Models.News;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Odds;
using Refit;

namespace LiveScore.Soccer.Services
{
    public static class SoccerApi
    {
        [Headers("Accept: application/x-msgpack")]
        public interface LeagueApi
        {
            [Get("/soccer/{language}/leagues/major")]
            Task<IEnumerable<League>> GetMajorLeagues(string language);

            [Get("/soccer/{language}/leagues/{leagueId}/seasons/{seasonId}/table/{leagueRoundGroup}")]
            Task<LeagueTable> GetTable(string language, string leagueId, string seasonId, string leagueRoundGroup);

            [Get("/soccer/{language}/leagues/{leagueId}/seasons/{seasonId}/matches/{leagueGroupName}")]
            Task<IEnumerable<SoccerMatch>> GetFixtures(string language, string leagueId, string seasonId, string leagueGroupName);

            [Get("/soccer/{language}/leagues/country/{countryCode}")]
            Task<IEnumerable<League>> GetCountryLeagues(string language, string countryCode);

            [Get("/soccer/{language}/leagues/{leagueId}/seasons/{seasonId}/groups")]
            Task<IEnumerable<LeagueGroupStage>> GetLeagueGroupStages(string language, string leagueId, string seasonId);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface MatchApi
        {
            [Get("/soccer/{language}/matches/live")]
            Task<IEnumerable<SoccerMatch>> GetLiveMatches(string language);

            [Get("/soccer/{language}/matches/live/count")]
            Task<int> GetLiveMatchesCount(string language);

            [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
            Task<IEnumerable<SoccerMatch>> GetMatches(string fromDate, string toDate, string language);

            [Get("/soccer/{language}/matches/{matchId}?eventDate={eventDate}")]
            Task<MatchInfo> GetMatchInfo(string matchId, string language, string eventDate);

            [Get("/soccer/{language}/matches/{matchId}/commentaries?eventDate={eventDate}")]
            Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, string language, string eventDate);

            [Get("/soccer/{language}/matches/{matchId}/statistic?eventDate={eventDate}")]
            Task<MatchStatistic> GetMatchStatistic(string matchId, string language, string eventDate);

            [Get("/soccer/{language}/matches/{matchId}/lineups?eventDate={eventDate}")]
            Task<MatchLineups> GetMatchLineups(string matchId, string language, string eventDate);

            [Post("/soccer/{language}/matches/ids")]
            Task<IEnumerable<SoccerMatch>> GetMatchByIds([Body]string[] ids, string language);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface OddsApi
        {
            [Get("/soccer/{lang}/odds/{matchId}/{betTypeId}/{formatType}")]
            Task<MatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType);

            [Get("/soccer/{lang}/odds-movement/{matchId}/{betTypeId}/{formatType}/{bookmakerId}")]
            Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, int betTypeId, string formatType, string bookmakerId);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface TeamApi
        {
            ///<example>soccer/en-US/teams/sr%3Acompetitor%3A22474/versus/sr%3Acompetitor%3A22595</example>
            ///<summary>GetHeadToHeads</summary>

            [Get("/soccer/{lang}/teams/{teamId1}/versus/{teamId2}")]
            Task<IEnumerable<SoccerMatch>> GetHeadToHeads(string lang, string teamId1, string teamId2);

            [Get("/soccer/{lang}/teams/{teamId}/results/?opponentTeamId={opponentTeamId}")]
            Task<IEnumerable<SoccerMatch>> GetTeamResults(string lang, string teamId, string opponentTeamId);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface NewsApi
        {
            [Get("/soccer/{language}/news")]
            Task<IEnumerable<NewsItem>> GetNews(string language);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface FavoriteCommandApi
        {
            [Post("/soccer/favorites/add")]
            Task AddFavorites(UserFavorite userFavorite);

            [Delete("/soccer/favorites/remove")]
            Task RemoveFavorite(string userId, string favoriteId);
        }
    }
}