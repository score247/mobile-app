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
        private readonly IApiService apiService;
        private readonly TeamApi teamApi;

        public TeamService(
            IApiService apiService,
            ILoggingService loggingService,
            TeamApi teamApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.teamApi = teamApi ?? apiService.GetApi<TeamApi>();
        }

        public async Task<IEnumerable<IMatch>> GetHeadToHeadsAsync(string teamId1, string teamId2, string language)
        {
            try
            {
                return await apiService.Execute(() => teamApi.GetHeadToHeads(language, teamId1, teamId2)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }
    }
}