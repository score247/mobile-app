using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Odds;
using LiveScore.Soccer.Models.Teams;
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
            Task<IEnumerable<SoccerMatch>> GetLiveMatches(string language);

            [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
            Task<IEnumerable<SoccerMatch>> GetMatches(string fromDate, string toDate, string language);

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

        [Headers("Accept: application/x-msgpack")]
        public interface TeamApi
        {
            ///soccer/en-US/teams/sr%3Acompetitor%3A22474/versus/sr%3Acompetitor%3A22595
            [Get("/soccer/{lang}/teams/{teamId1}/versus/{teamId2}")]
            Task<HeadToHeads> GetHeadToHeads(string lang, string teamId1, string teamId2);
        }
    }
}