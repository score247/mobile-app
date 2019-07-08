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
        [Get("/soccer/Odds/Get?matchId={matchId}&betTypeId={betTypeId}")]
        Task<MatchOdds> GetOdds(string matchId, int betTypeId);
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

        public async Task<IMatchOdds> GetOdds(string matchId, int betTypeId, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = cacheService.CacheDuration(CacheDurationTerm.Long);
                var cacheKey = $"Odds-{matchId}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetOddsFromApi(matchId, betTypeId),
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

        private async Task<MatchOdds> GetOddsFromApi(string matchId, int betTypeId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(matchId, betTypeId)
           );
    }
}
