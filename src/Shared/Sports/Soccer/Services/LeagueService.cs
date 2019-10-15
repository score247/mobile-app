using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Caching;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using MethodTimer;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public class LeagueService : BaseService, ILeagueService
    {
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;

        public LeagueService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
        }

        public async Task<IEnumerable<ILeague>> GetMajorLeagues(bool getLatestData = false)
        {
            try
            {
                const string cacheKey = "LiveScore.Soccer.Services.LeagueService.MajorLeagues";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    GetMajorLeaguesFromApi,
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)),
                    getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<ILeague>();
            }
        }

        [Time]
        private Task<IEnumerable<ILeague>> GetMajorLeaguesFromApi()
            => apiService.Execute(() => apiService.GetApi<LeagueApi>().GetMajorLeagues());
    }
}