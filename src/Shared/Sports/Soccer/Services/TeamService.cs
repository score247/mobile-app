using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public class TeamService : BaseService, ITeamService
    {
        private const int LongDuration = 7_200;

        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly TeamApi teamApi;

        public TeamService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            TeamApi teamApi = null) : base(loggingService)
        {
            this.cacheManager = cacheManager;
            this.apiService = apiService;
            this.teamApi = teamApi ?? apiService.GetApi<TeamApi>();
        }

        public async Task<IEnumerable<IMatch>> GetHeadToHeadsAsync(string teamId1, string teamId2, string language, bool forceFetchLatestData = false)
        {
            try
            {
                var cacheKey = $"TeamService.GetHeadToHeadsAsync:{teamId1}:{teamId2}:{language}";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => teamApi.GetHeadToHeads(language, teamId1, teamId2)),
                    LongDuration,
                    forceFetchLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }
    }
}