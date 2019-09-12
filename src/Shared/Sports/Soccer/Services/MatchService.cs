namespace LiveScore.Soccer.Services
{
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

    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
        Task<IEnumerable<Match>> GetMatches(string fromDate, string toDate, string language);

        [Get("/soccer/{language}/matches/live?lq=lastUpdateMatchStatusTime")]
        Task<IEnumerable<Match>> GetLiveMatches(Language language, DateTime lastUpdateMatchStatusTime);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<MatchInfo> GetMatchInfo(string matchId, string language);
    }

    public interface IMatchInfoService
    {
        Task<MatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false);
    }

    public class MatchService : BaseService, IMatchService, IMatchInfoService
    {
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;

        public MatchService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
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
                    () => GetMatchesFromApi(
                            dateTime.BeginningOfDay().ToApiFormat(),
                            dateTime.EndOfDay().ToApiFormat(),
                            language.DisplayName), (int)cacheDuration,
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
                    (int)CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        [Time]
        private Task<IEnumerable<Match>> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatches(fromDateText, toDateText, language));

        [Time]
        private Task<MatchInfo> GetMatchFromApi(string matchId, string language)
           => apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatchInfo(matchId, language));

        public Task<IEnumerable<IMatch>> GetLiveMatches(Language language, DateTime lastUpdateMatchStatusTime, bool getLatestData = false)
        {
            // TODO: add impl later

            //var cacheKey = $"LiveMatches::{language.DisplayName}";

            return GetMatchesByDate(DateTime.Now, language, getLatestData);
        }
    }
}