namespace LiveScore.Soccer.Services
{
    using Fanex.Caching;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Odds;
    using Refit;
    using System;
    using System.Threading.Tasks;

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
        private readonly ICacheService cacheService;

        public OddsService(
            ICacheService cacheService,
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
                    await cacheService.RemoveAsync(oddDataCacheKey);                   
                }

                var matchOdds = await GetOddsFromApi(lang, matchId, betTypeId, formatType);

                if (matchOdds == null)
                {
                    matchOdds = await cacheService.GetAsync<MatchOdds>(oddDataCacheKey);
                }
                else
                {
                    await cacheService.SetAsync(oddDataCacheKey, matchOdds, new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)CacheDuration.Long)));
                }              

                return matchOdds;
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

        public async Task<IMatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool forceFetchNewData = false)
        {
            try
            {
                var oddMovementCacheKey = $"{OddsMovementKey}-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";

                if (forceFetchNewData)
                {
                    await cacheService.RemoveAsync(oddMovementCacheKey);
                }

                var matchOddsMovement = await GetOddsMovementFromApi(lang, matchId, betTypeId, formatType, bookmakerId);

                if (matchOddsMovement == null)
                {
                    matchOddsMovement = await cacheService.GetAsync<MatchOddsMovement>(oddMovementCacheKey);                    
                }
                else
                {
                    await cacheService.SetAsync(oddMovementCacheKey, matchOddsMovement, new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds((double)CacheDuration.Long)));
                }

                return matchOddsMovement;                
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