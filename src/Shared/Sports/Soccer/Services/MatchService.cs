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
        Task<MatchInfo> GetMatchAsync(string matchId, Language language, bool forceFetchLatestData = false);

        Task<MatchCoverage> GetMatchCoverageAsync(string matchId, Language language, bool forceFetchLatestData = false);

        Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language, bool forceFetchLatestData = false);

        Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language, bool forceFetchLatestData = false);



    }

    public class MatchService : BaseService, IMatchService, ISoccerMatchService
    {
        private const int ShortDuration = 120;
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
        public async Task<IEnumerable<IMatch>> GetMatchesByDateAsync(DateTime dateTime, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"Matches:{dateTime.Date}:{language.DisplayName}";

                var cacheDuration = dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date
                    ? ShortDuration
                    : LongDuration;

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => matchApi.GetMatches(
                        dateTime.BeginningOfDay().ToApiFormat(),
                        dateTime.EndOfDay().ToApiFormat(),
                        language.DisplayName)), cacheDuration,
                    forceFetchLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<MatchInfo> GetMatchAsync(string matchId, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"SoccerMatch:{matchId}:{language}";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => matchApi.GetMatchInfo(matchId, language.DisplayName)),
                    ShortDuration, forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchInfo();
            }
        }

        public async Task<IEnumerable<IMatch>> GetLiveMatchesAsync(Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"LiveMatches::{language.DisplayName}";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => matchApi.GetLiveMatches(language.DisplayName)),
                        ShortDuration, forceFetchLatestData)
                    .ConfigureAwait(false);
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
                var cacheKey = $"SoccerMatch:{matchId}:{language}:coverage";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => matchApi.GetMatchCoverage(matchId, language.DisplayName)),
                        ShortDuration, forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchCoverage { MatchId = matchId };
            }
        }

        public async Task<IEnumerable<MatchCommentary>> GetMatchCommentariesAsync(string matchId, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"SoccerMatch:{matchId}:{language}:Commentaries";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => matchApi.GetMatchCommentaries(matchId, language.DisplayName)),
                        ShortDuration, forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<MatchCommentary>();
            }
        }

        public async Task<MatchStatistic> GetMatchStatisticAsync(string matchId, Language language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"SoccerMatch:{matchId}:statistic";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => matchApi.GetMatchStatistic(matchId, language.DisplayName)),
                    ShortDuration,
                    forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchStatistic(matchId);
            }
        }
    }
}