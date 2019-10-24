using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public class LeagueService : BaseService, ILeagueService
    {
        private const int CacheDuration = 86_400;
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly LeagueApi leagueApi;

        public LeagueService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            LeagueApi leagueApi) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
            this.leagueApi = leagueApi ?? apiService.GetApi<LeagueApi>();
        }

        public async Task<IEnumerable<ILeague>> GetMajorLeaguesAsync(bool forceFetchLatestData = false)
        {
            try
            {
                const string cacheKey = "LiveScore.Soccer.Services.LeagueService.MajorLeagues";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                     () => apiService.Execute(() => leagueApi.GetMajorLeagues()),
                     CacheDuration,
                    forceFetchLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<ILeague>();
            }
        }
    }
}