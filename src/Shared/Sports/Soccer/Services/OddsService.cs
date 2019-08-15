namespace LiveScore.Soccer.Services
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
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

    public class OddsService : BaseService, IOddsService
    {
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

        public async Task<IMatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType, bool forceFetchNewData = false)
        {
            try
            {
                var oddDataCacheKey = $"Odds-{matchId}-{betTypeId}-{formatType}";

                return await cacheService.GetAndFetchLatestValue(
                        oddDataCacheKey,
                        () => GetOddsFromApi(lang, matchId, betTypeId, formatType),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Long));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOdds();
            }
        }

        private async Task<MatchOdds> GetOddsFromApi(string lang, string matchId, int betTypeId, string formatType)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(lang, matchId, betTypeId, formatType)
           );

        public async Task<IMatchOddsMovement> GetOddsMovement(string lang, string matchId, int betTypeId, string formatType, string bookmakerId, bool forceFetchNewData = false)
        {
            try
            {
                var oddMovementCacheKey = $"OddsMovement-{matchId}-{betTypeId}-{formatType}-{bookmakerId}";

                return await cacheService.GetAndFetchLatestValue(
                        oddMovementCacheKey,
                        () => GetOddsMovementFromApi(lang, matchId, betTypeId, formatType, bookmakerId),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Long));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOddsMovement();
            }
        }

        private async Task<MatchOddsMovement> GetOddsMovementFromApi(string lang, string matchId, int betTypeId, string formatType, string bookmakerId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOddsMovement(lang, matchId, betTypeId, formatType, bookmakerId)
           );
    }
}