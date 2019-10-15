namespace LiveScore.Soccer.Services
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Odds;
    using static LiveScore.Soccer.Services.SoccerApi;

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
        private const string OddsComparisonKey = "LiveScore.Soccer.Services.OddsComparison";
        private const string OddsMovementKey = "LiveScore.Soccer.Services.OddsMovement";
        private const int CacheDuration = 7_200;

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly OddsApi oddsApi;

        public OddsService(
            ICacheManager cacheManager,
            ILoggingService loggingService,
            IApiService apiService,
            OddsApi oddsApi = null) : base(loggingService)
        {
            this.cacheManager = cacheManager;
            this.apiService = apiService;
            this.oddsApi = oddsApi ?? apiService.GetApi<OddsApi>();
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
                        () => apiService.Execute(() => oddsApi.GetOdds(lang, matchId, betTypeId, formatType)), CacheDuration,
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
                   () => apiService.Execute(() => oddsApi.GetOddsMovement(
                       lang,
                       matchId,
                       betTypeId,
                       formatType,
                       bookmakerId)), CacheDuration,
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