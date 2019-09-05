namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Caching;
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

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<MatchInfo> GetMatchInfo(string matchId, string language);
    }

    public class MatchService : BaseService, IMatchService
    {
        private readonly IApiService apiService;
        private readonly ICacheService cacheService;

        public MatchService(
            IApiService apiService,
            ICacheService cacheService,
            ILoggingService loggingService) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheService = cacheService;
        }

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatches(DateRange dateRange, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var dataFromDate = await GetMatchesByDate(dateRange.From, language, forceFetchNewData).ConfigureAwait(false);

                if (dateRange.IsOneDay)
                {
                    return dataFromDate;
                }

                var dataToDate = await GetMatchesByDate(dateRange.To, language, forceFetchNewData).ConfigureAwait(false);

                return dataFromDate.Concat(dataToDate);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        private async Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Matches:{dateTime.Date}:{language.DisplayName}";

                if(forceFetchNewData)
                {
                    await cacheService.RemoveAsync(cacheKey);
                }

                var cacheDuration = dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date
                    ? CacheDuration.Short
                    : CacheDuration.Long;

                return await cacheService.GetOrSetAsync(
                    cacheKey,
                    () => GetMatchesFromApi(
                            dateTime.BeginningOfDay().ToApiFormat(),
                            dateTime.EndOfDay().ToApiFormat(),
                            language.DisplayName),
                    new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)cacheDuration)));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<IMatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}";

                if (forceFetchNewData)
                {
                    await cacheService.RemoveAsync(cacheKey);
                }

                return await cacheService.GetOrSetAsync(
                    cacheKey,
                    () => GetMatchFromApi(matchId, language.DisplayName),
                    new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)CacheDuration.Short)));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        [Time]
        private IEnumerable<Match> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatches(fromDateText, toDateText, language)).GetAwaiter().GetResult();

        [Time]
        private MatchInfo GetMatchFromApi(string matchId, string language)
           => apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatchInfo(matchId, language)).GetAwaiter().GetResult();
    }
}