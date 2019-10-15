using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Odds;
using Refit;

namespace LiveScore.Soccer.Services
{
    public static class SoccerApi
    {
        public interface LeagueApi
        {
            [Get("/soccer/{language}/leagues/major")]
            Task<IEnumerable<ILeague>> GetMajorLeagues();
        }

        [Headers("Accept: application/x-msgpack")]
        public interface MatchApi
        {
            [Get("/soccer/{language}/matches/live")]
            Task<IEnumerable<Match>> GetLiveMatches(string language);

            [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
            Task<IEnumerable<Match>> GetMatches(string fromDate, string toDate, string language);

            [Get("/soccer/{language}/matches/{matchId}")]
            Task<MatchInfo> GetMatchInfo(string matchId, string language);

            [Get("/soccer/{language}/matches/{matchId}/coverage")]
            Task<MatchCoverage> GetMatchCoverage(string matchId, string language);

            [Get("/soccer/{language}/matches/{matchId}/commentaries")]
            Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, string language);

            [Get("/soccer/{language}/matches/{matchId}/statistic")]
            Task<MatchStatistic> GetMatchStatistic(string matchId, string language);
        }

        [Headers("Accept: application/x-msgpack")]
        public interface OddsApi
        {
            [Get("/soccer/{lang}/odds/{matchId}/{betTypeId}/{formatType}")]
            Task<MatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType);

            [Get("/soccer/{lang}/odds-movement/{matchId}/{betTypeId}/{formatType}/{bookmakerId}")]
            Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, int betTypeId, string formatType, string bookmakerId);
        }
    }
}