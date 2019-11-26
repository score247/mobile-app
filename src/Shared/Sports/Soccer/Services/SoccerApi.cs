﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;
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

            [Get("/soccer/{language}/leagues/{leagueId}/season/{seasonId}/table/{leagueRoundGroup}")]
            Task<LeagueTable> GetTable(string language, string leagueId, string seasonId, string leagueRoundGroup);
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

            [Get("/soccer/{language}/matches/{matchId}/lineups")]
            Task<MatchLineups> GetMatchLineups(string matchId, string language);
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
    }
}