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

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<MatchInfo> GetMatchInfo(string matchId, string language);
    }

    public class MatchService : BaseService, IMatchService
    {
        private readonly IApiService apiService;
        private readonly ICachingService cacheService;

        public MatchService(
            IApiService apiService,
            ICachingService cacheService,
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
                else
                {
                    var dataToDate = await GetMatchesByDate(dateRange.To, language, forceFetchNewData);

                    return dataFromDate.Concat(dataToDate);
                }
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

                if (dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date
                    || forceFetchNewData)
                {
                    return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatchesFromApi(
                            dateTime.BeginningOfDay().ToApiFormat(),
                            dateTime.EndOfDay().ToApiFormat(),
                            language.DisplayName),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Short)).ConfigureAwait(false);
                }

                return await cacheService.GetOrFetchValue(
                       cacheKey,
                       () => GetMatchesFromApi(
                           dateTime.BeginningOfDay().ToApiFormat(),
                           dateTime.EndOfDay().ToApiFormat(),
                           language.DisplayName), DateTime.Now.AddSeconds((int)CacheDuration.Long))
                    .ConfigureAwait(false);
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

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatchFromApi(matchId, language.DisplayName),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Short))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        [Time]
        private async Task<IEnumerable<Match>> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => await apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatches(fromDateText, toDateText, language))
            .ConfigureAwait(false);

        [Time]
        private async Task<MatchInfo> GetMatchFromApi(string matchId, string language)
           => await apiService.Execute(() => apiService.GetApi<ISoccerMatchApi>().GetMatchInfo(matchId, language)).ConfigureAwait(false);
    }
}