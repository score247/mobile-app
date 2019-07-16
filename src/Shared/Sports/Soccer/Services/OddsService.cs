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
        [Get("/soccer/{lang}/Odds/{matchId}/{betTypeId}/{formatType}")]
        Task<MatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType);
    }

    public class OddsService : BaseService, IOddsService
    {
        private readonly IApiService apiService;
        private readonly ILocalStorage cacheService;

        public OddsService(
            ILocalStorage cacheService,
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
                var cacheExpiration = cacheService.CacheDuration(CacheDurationTerm.Long);
                var cacheKey = $"Odds-{matchId}-{betTypeId}-{formatType}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetOddsFromApi(lang, matchId, betTypeId, formatType),
                        (offset) =>
                        {
                            if (forceFetchNewData)
                            {
                                return true;
                            }

                            var elapsed = DateTimeOffset.Now - offset;

                            return elapsed > cacheExpiration;
                        });
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
    }
}
