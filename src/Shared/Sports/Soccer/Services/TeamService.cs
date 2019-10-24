using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public class TeamService : BaseService, ITeamService
    {
        private const int ShortDuration = 120;
        private const int LongDuration = 7_200;

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly TeamApi teamApi;

        public TeamService(
        IApiService apiService,
        ICacheManager cacheManager,
        ILoggingService loggingService,
        TeamApi teamApi = null) : base(loggingService)
        {
            this.cacheManager = cacheManager;
            this.apiService = apiService;
            this.teamApi = teamApi ?? apiService.GetApi<TeamApi>();
        }

        public async Task<IHeadToHeads> GetHeadToHeadsAsync(string teamId1, string teamId2, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"HeadToHead:{teamId1}:{teamId2}:{language.DisplayName}";
               
                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => teamApi.GetHeadToHeads(language.DisplayName, teamId1, teamId2)),
                    LongDuration,
                    forceFetchLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Got Exception");
                HandleException(ex);

                return null;
            }
        }

        private Task<IHeadToHeads> StubApi(string teamId1, string teamId2, Language language, bool forceFetchLatestData = false)
            => Task.FromResult<IHeadToHeads>(
                new HeadToHeads
                (
                    new List<Team>
                    {
                        new Team{ Id = teamId1, IsHome = true },
                        new Team{ Id = teamId2, IsHome = false }
                    },
                    new List<SoccerMatch>
                    {
                        StubNotStartedMatch("match:1", teamId1, teamId2, "season:1", StubLeague("league:1", "English Premier League", 1, "England", "UK", false)),

                        StubClosedMatch("match:2", teamId1, teamId2, "season:1",
                            StubMatchResult(teamId1, 2, 1),
                            StubLeague("league:1", "English Premier League", 1, "England", "UK", false)),

                        StubClosedMatch("match:3", teamId1, teamId2, "season:2",
                            StubMatchResult(teamId2, 2, 3),
                            StubLeague("league:1", "Champions League", 1, "", "", true))
                    }
                ));



        private SoccerMatch StubNotStartedMatch(
            string matchId,
            string teamId1,
            string teamId2,
            string seasonId,
            League league)
        => new SoccerMatch
            (
                id: matchId,
                eventDate: DateTimeOffset.Now.AddDays(3),
                currentPeriodStartTime: DateTimeOffset.Now,
                leagueId: league.Id,
                leagueName: league.Name,
                homeTeamId: teamId1,
                homeTeamName: "home team",
                awayTeamId: teamId2,
                awayTeamName: "away team",
                matchStatus: MatchStatus.NotStarted,
                eventStatus: MatchStatus.NotStarted,
                homeScore: 0,
                awayScore: 0,
                winnerId: null,
                aggregateWinnerId: null,
                aggregateHomeScore: 0,
                aggregateAwayScore: 0,
                homeRedCards: 0,
                homeYellowRedCards: 0,
                awayRedCards: 0,
                awayYellowRedCards: 0,
                matchTime: 0,
                stoppageTime: "0",
                injuryTimeAnnounced: 0,
                lastTimelineType: null,
                matchPeriods: null,
                countryCode: league.CountryCode,
                countryName: league.CountryName,
                modifiedTime: DateTimeOffset.Now,
                isInternationalLeague: league.IsInternational,
                leagueOrder: league.Order,
                seasonId: seasonId
            );

        private SoccerMatch StubClosedMatch(
            string matchId,
            string teamId1,
            string teamId2,
            string seasonId,
            MatchResult matchResult,
            League league)
        => new SoccerMatch
            (
                id: matchId,
                eventDate: DateTimeOffset.Now.AddDays(-3),
                currentPeriodStartTime: DateTimeOffset.Now,
                leagueId: league.Id,
                leagueName: league.Name,
                homeTeamId: teamId1,
                homeTeamName: $"team {teamId1}",
                awayTeamId: teamId2,
                awayTeamName: $"team {teamId2}",
                matchStatus: matchResult.MatchStatus,
                eventStatus: matchResult.EventStatus,
                homeScore: matchResult.HomeScore,
                awayScore: matchResult.AwayScore,
                winnerId: matchResult.WinnerId,
                aggregateWinnerId: null,
                aggregateHomeScore: 0,
                aggregateAwayScore: 0,
                homeRedCards: 0,
                homeYellowRedCards: 0,
                awayRedCards: 0,
                awayYellowRedCards: 0,
                matchTime: 0,
                stoppageTime: "0",
                injuryTimeAnnounced: 0,
                lastTimelineType: null,
                matchPeriods: matchResult.MatchPeriods,
                countryCode: league.CountryCode,
                countryName: league.CountryName,
                modifiedTime: DateTimeOffset.Now.AddDays(-3),
                isInternationalLeague: league.IsInternational,
                leagueOrder: league.Order,
                seasonId: seasonId
            );

        private League StubLeague(
            string id,
            string name,
            int order,
            string countryName,
            string countryCode,
            bool isInternational)
        => new League(
            id,
            name,
            order,
            null,
            countryName,
            countryCode,
            isInternational);

        private MatchResult StubMatchResult(
            string winnerId,
            byte homeScore,
            byte awayScore)
            => new MatchResult(
                matchStatus: MatchStatus.Ended,
                eventStatus: MatchStatus.Closed,
                2,
                new List<MatchPeriod>(),
                matchTime: 90,
                winnerId: winnerId,
                homeScore: homeScore,
                awayScore: awayScore,
                0,
                0,
                ""
                );
    }
}
