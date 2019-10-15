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
using Refit;

namespace LiveScore.Soccer.Services
{
    [Headers("Accept: application/x-msgpack")]
    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
        Task<IEnumerable<Match>> GetMatches(string fromDate, string toDate, string language);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<MatchInfo> GetMatchInfo(string matchId, string language);

        [Get("/soccer/{language}/matches/live")]
        Task<IEnumerable<Match>> GetLiveMatches(string language);

        [Get("/soccer/{language}/matches/{matchId}/coverage")]
        Task<MatchCoverage> GetMatchCoverage(string matchId, string language);

        [Get("/soccer/{language}/matches/{matchId}/commentaries")]
        Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, string language);

        [Get("/soccer/{language}/matches/{matchId}/statistic")]
        Task<MatchStatistic> GetMatchStatistic(string matchId, string language);
    }

    public interface IMatchInfoService
    {
        Task<MatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false);

        Task<MatchCoverage> GetMatchCoverage(string matchId, Language language, bool forceFetchNewData = false);

        Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, Language language, bool forceFetchNewData = false);

        Task<MatchStatistic> GetMatchStatistic(string matchId, bool forceFetchNewData = false);
    }

    public class MatchService : BaseService, IMatchService, IMatchInfoService
    {
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly ISoccerMatchApi soccerMatchApi;

        public MatchService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            ISoccerMatchApi soccerMatchApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
            this.soccerMatchApi = soccerMatchApi ?? apiService.GetApi<ISoccerMatchApi>();
        }

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool getLatestData = false)
        {
            try
            {
                var cacheKey = $"Matches:{dateTime.Date}:{language.DisplayName}";

                var cacheDuration = dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date
                    ? CacheDuration.Short
                    : CacheDuration.Long;

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => soccerMatchApi.GetMatches(
                        dateTime.BeginningOfDay().ToApiFormat(),
                        dateTime.EndOfDay().ToApiFormat(),
                        language.DisplayName)), cacheDuration,
                    getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<MatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => GetMatchFromApi(matchId, language.DisplayName),
                    CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        public async Task<IEnumerable<IMatch>> GetLiveMatches(Language language, bool getLatestData = false)
        {
            try
            {
                var cacheKey = $"LiveMatches::{language.DisplayName}";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => GetLiveMatchesFromApi(language.DisplayName),
                        CacheDuration.Short, getLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<MatchCoverage> GetMatchCoverage(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}:coverage";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => GetMatchCoverageFromApi(matchId, language.DisplayName),
                        CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchCoverage { MatchId = matchId };
            }
        }

        public async Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}:Commentaries";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => GetMatchCommentariesFromApi(matchId, language.DisplayName),
                        CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<MatchCommentary>();
            }
        }

        [Time]
        private Task<IEnumerable<Match>> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => apiService.Execute(() => soccerMatchApi.GetMatches(fromDateText, toDateText, language));

        [Time]
        private Task<MatchInfo> GetMatchFromApi(string matchId, string language)
            => apiService.Execute(() => soccerMatchApi.GetMatchInfo(matchId, language));

        [Time]
        private Task<IEnumerable<Match>> GetLiveMatchesFromApi(string language)
            => apiService.Execute(() => soccerMatchApi.GetLiveMatches(language));

        [Time]
        private Task<MatchCoverage> GetMatchCoverageFromApi(string matchId, string language)
            => apiService.Execute(() => soccerMatchApi.GetMatchCoverage(matchId, language));

        [Time]
        private Task<IEnumerable<MatchCommentary>> GetMatchCommentariesFromApi(string matchId, string language)
            => apiService.Execute(() => soccerMatchApi.GetMatchCommentaries(matchId, language));

        public async Task<MatchStatistic> GetMatchStatistic(string matchId, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:statistic";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => GetMatchStatisticFromApi(matchId),
                    CacheDuration.Short,
                    forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchStatistic(matchId);
            }
        }

        [Time]
        private Task<MatchStatistic> GetMatchStatisticFromApi(string matchId)
            => apiService.Execute(() => soccerMatchApi.GetMatchStatistic(matchId, Language.English.DisplayName));
    }
}