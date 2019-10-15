namespace LiveScore.Soccer.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Caching;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Odds;
    using Refit;

    [Headers("Accept: application/x-msgpack")]
    public interface ISoccerOddsApi
    {
        [Get("/soccer/{lang}/odds/{matchId}/{betTypeId}/{formatType}")]
        Task<MatchOdds> GetOdds(
            string lang,
            string matchId,
            int betTypeId,
            string formatType);

        [Get("/soccer/{lang}/odds-movement/{matchId}/{betTypeId}/{formatType}/{bookmakerId}")]
        Task<MatchOddsMovement> GetOddsMovement(
            string lang,
            string matchId,
            int betTypeId,
            string formatType,
            string bookmakerId);
    }

    public interface IOddsService
    {
        Task<MatchOdds> GetOddsAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            bool getLatestData = false);

        Task<MatchOddsMovement> GetOddsMovementAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            string bookmakerId,
            bool getLatestData = false);
    }

    public class OddsService : BaseService, IOddsService
    {
        private const string OddsComparisonKey = "OddsComparison";
        private const string OddsMovementKey = "OddsMovement";
        private const int CacheDuration = 7_200;

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly ISoccerOddsApi soccerOddsApi;

        public OddsService(
            ICacheManager cacheManager,
            ILoggingService loggingService,
            IApiService apiService,
            ISoccerOddsApi soccerOddsApi = null) : base(loggingService)
        {
            this.cacheManager = cacheManager;
            this.apiService = apiService;
            this.soccerOddsApi = soccerOddsApi ?? apiService.GetApi<ISoccerOddsApi>();
        }

        public async Task<MatchOdds> GetOddsAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            bool getLatestData = false)
        {
            try
            {
                var oddDataCacheKey = $"{OddsComparisonKey}-{matchId}-{betTypeId}-{formatType}";

                return await cacheManager
                    .GetOrSetAsync(
                        oddDataCacheKey,
                        () => apiService.Execute(() => soccerOddsApi.GetOdds(lang, matchId, betTypeId, formatType)),
                        new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheDuration)),
                        getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOdds();
            }
        }

        public async Task<MatchOddsMovement> GetOddsMovementAsync(
            string lang,
            string matchId,
            byte betTypeId,
            string formatType,
            string bookmakerId,
            bool getLatestData = false)
        {
            try
            {
                var oddMovementCacheKey = $"{OddsMovementKey}-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";

                return await cacheManager.GetOrSetAsync(
                   oddMovementCacheKey,
                   () => apiService.Execute(() => soccerOddsApi.GetOddsMovement(
                       lang,
                       matchId,
                       betTypeId,
                       formatType,
                       bookmakerId)),
                   new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheDuration)),
                   getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOddsMovement();
            }
        }
    }
}