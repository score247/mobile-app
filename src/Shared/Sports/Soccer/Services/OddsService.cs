namespace LiveScore.Soccer.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using MethodTimer;
    using Refit;

    public interface ISoccerOddsApi
    {
        [Get("/soccer/{lang}/odds/{matchId}/{betTypeId}/{formatType}")]
        Task<MatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType);

        [Get("/soccer/{lang}/odds-movement/{matchId}/{betTypeId}/{formatType}/{bookmakerId}")]
        Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, int betTypeId, string formatType, string bookmakerId);
    }

    public class OddsService : BaseService, IOddsService
    {
        private const string OddsComparisonKey = "OddsComparison";
        private const string OddsMovementKey = "OddsMovement";

        private readonly IApiService apiService;
        private readonly ICachingService cacheService;

        public OddsService(
            ICachingService cacheService,
            ILoggingService loggingService,
            IApiService apiService
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.apiService = apiService;
        }

        public async Task<IMatchOdds> GetOdds(string lang, string matchId, byte betTypeId, string formatType, bool forceFetchNewData = false)
        {
            try
            {
                var oddDataCacheKey = $"{OddsComparisonKey}-{matchId}-{betTypeId}-{formatType}";

                if (forceFetchNewData)
                {
                    return await cacheService.GetAndFetchLatestValue(
                        oddDataCacheKey,
                        () => GetOddsFromApi(lang, matchId, betTypeId, formatType),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Long));
                }

                return await cacheService.GetOrFetchValue(
                        oddDataCacheKey,
                        () => GetOddsFromApi(lang, matchId, betTypeId, formatType),
                        DateTime.Now.AddSeconds((int)CacheDuration.Long));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOdds();
            }
        }

        private async Task<MatchOdds> GetOddsFromApi(string lang, string matchId, byte betTypeId, string formatType)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(lang, matchId, betTypeId, formatType)
           );

        public async Task<IMatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool forceFetchNewData = false)
        {
            try
            {
                var oddMovementCacheKey = $"{OddsMovementKey}-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";

                if (forceFetchNewData)
                {
                    return await cacheService.GetAndFetchLatestValue(
                        oddMovementCacheKey,
                        () => GetOddsMovementFromApi(lang, matchId, betTypeId, formatType, bookmakerId),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Long));
                }

                return await cacheService.GetOrFetchValue(
                        oddMovementCacheKey,
                        () => GetOddsMovementFromApi(lang, matchId, betTypeId, formatType, bookmakerId),
                        DateTime.Now.AddSeconds((int)CacheDuration.Long));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOddsMovement();
            }
        }

        private async Task<MatchOddsMovement> GetOddsMovementFromApi(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOddsMovement(lang, matchId, betTypeId, formatType, bookmakerId)
           );

        [Time]
        public void InvalidateAllOddsComparisonCache(string matchId)
        {
            Debug.WriteLine($"InvalidateAllOddsComparisonCache {matchId}");
            foreach (var betType in Enumeration.GetAll<BetType>())
            {
                Parallel.ForEach(
                    Enumeration.GetAll<OddsFormat>(),
                    async (format) =>
                    {
                        await InvalidateOddsComparisonCache(matchId, betType.Value, format.DisplayName);
                    });
            }
        }

        public async Task InvalidateOddsComparisonCache(string matchId, byte betTypeId, string formatType)
        {
            try
            {
                var cacheKey = $"{OddsComparisonKey}-{matchId}-{betTypeId}-{formatType}";
                Debug.WriteLine($"InvalidateOddsComparisonCache {cacheKey}");

                await cacheService.Invalidate(cacheKey);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task InvalidateOddsMovementCache(string matchId, byte betTypeId, string formatType, string bookmakerId)
        {
            try
            {
                var cacheKey = $"{OddsMovementKey}-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";
                Debug.WriteLine($"InvalidateOddsMovementCache {cacheKey}");

                await cacheService.Invalidate(cacheKey);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}