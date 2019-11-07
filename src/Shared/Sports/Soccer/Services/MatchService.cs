using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using MethodTimer;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public interface ISoccerMatchService
    {
        Task<MatchInfo> GetMatchAsync(string matchId, Language language);

        Task<MatchCoverage> GetMatchCoverageAsync(string matchId, Language language, bool forceFetchLatestData = false);

        Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language);

        Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language);

        Task<MatchLineups> GetMatchLineupsAsync(string matchId, Language language);
    }

    public class MatchService : BaseService, IMatchService, ISoccerMatchService
    {
        private const int LongDuration = 7_200;

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly MatchApi matchApi;

        public MatchService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            MatchApi matchApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
            this.matchApi = matchApi ?? apiService.GetApi<MatchApi>();
        }

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatchesByDateAsync(DateTime dateTime, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatches(
                    dateTime.BeginningOfDay().ToApiFormat(),
                    dateTime.EndOfDay().ToApiFormat(),
                    language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<MatchInfo> GetMatchAsync(string matchId, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchInfo(matchId, language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchInfo();
            }
        }

        public async Task<IEnumerable<IMatch>> GetLiveMatchesAsync(Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetLiveMatches(language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<MatchCoverage> GetMatchCoverageAsync(string matchId, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"SoccerMatch:{matchId}:coverage";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => matchApi.GetMatchCoverage(matchId, language.DisplayName)),
                        LongDuration, forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchCoverage { MatchId = matchId };
            }
        }

        public async Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchCommentaries(matchId, language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<MatchCommentary>();
            }
        }

        public async Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchStatistic(matchId, language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchStatistic(matchId);
            }
        }

        public async Task<MatchLineups> GetMatchLineupsAsync(string matchId, Language language)
        {
            try
            {
                return await apiService.Execute(() => matchApi.GetMatchLineups(matchId, language.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchLineups(matchId);
            }
        }
    }
}