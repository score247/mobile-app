namespace LiveScore.Soccer.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Caching;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Odds;
    using Refit;

    public interface ISoccerOddsApi
    {
        [Get("/soccer/{lang}/odds/{matchId}/{betTypeId}/{formatType}")]
        Task<MatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType);

        [Get("/soccer/{lang}/odds-movement/{matchId}/{betTypeId}/{formatType}/{bookmakerId}")]
        Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, int betTypeId, string formatType, string bookmakerId);
    }

    public interface IOddsService
    {
        Task<MatchOdds> GetOdds(string lang, string matchId, byte betTypeId, string formatType, bool forceFetchNewData = false);

        Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool forceFetchNewData = false);
    }

    public class OddsService : BaseService, IOddsService
    {
        private const string OddsComparisonKey = "OddsComparison";
        private const string OddsMovementKey = "OddsMovement";

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;

        public OddsService(
            ICacheManager cacheManager,
            ILoggingService loggingService,
            IApiService apiService
            ) : base(loggingService)
        {
            this.cacheManager = cacheManager;
            this.apiService = apiService;
        }

        public async Task<MatchOdds> GetOdds(string lang, string matchId, byte betTypeId, string formatType, bool getLatestData = false)
        {
            try
            {
                var oddDataCacheKey = $"{OddsComparisonKey}-{matchId}-{betTypeId}-{formatType}";

                return await cacheManager.GetOrSetAsync(
                    oddDataCacheKey,
                    () => GetOddsFromApi(lang, matchId, betTypeId, formatType),
                    new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)CacheDuration.Long)),
                    getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOdds();
            }
        }

        private Task<MatchOdds> GetOddsFromApi(string lang, string matchId, byte betTypeId, string formatType)
           => apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(lang, matchId, betTypeId, formatType)
           );

        public async Task<MatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool getLatestData = false)
        {
            try
            {
                var oddMovementCacheKey = $"{OddsMovementKey}-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";

                return await cacheManager.GetOrSetAsync(
                   oddMovementCacheKey,
                   () => GetOddsMovementFromApi(lang, matchId, betTypeId, formatType, bookmakerId),
                   new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)CacheDuration.Long)),
                   getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOddsMovement();
            }
        }

        private Task<MatchOddsMovement> GetOddsMovementFromApi(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId)
           => apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOddsMovement(lang, matchId, betTypeId, formatType, bookmakerId)
           );
    }
}