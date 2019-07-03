namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using Refit;

    public interface ISoccerOddsApi
    {
        [Get("/Odds/Get?sportId={sportId}&matchId={matchId}&betTypeId={betTypeId}")]
        Task<MatchOdds> GetOdds(string sportId, string matchId, int betTypeId);
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

        public async Task<IMatchOdds> GetOdds(UserSettings settings, string matchId, int betTypeId, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = cacheService.CacheDuration(CacheDurationTerm.Long);
                var cacheKey = $"Odds-{matchId}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,                       
                        () => GetOddsFromApi(settings.SportId, matchId, betTypeId),
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

        private async Task<MatchOdds> GetOddsFromApi(string sportId, string matchId, int betTypeId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(sportId, matchId, betTypeId)
           );
    }
}
