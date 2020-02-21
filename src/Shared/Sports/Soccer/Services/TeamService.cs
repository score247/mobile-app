using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
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
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetHeadToHeadsAsync)},
                    { "TeamId1", teamId1},
                    { "TeamId2", teamId2}
                };

                HandleException(ex, properties);

                return null;
            }
        }

        public async Task<IEnumerable<IMatch>> GetTeamResultsAsync(string teamId, string opponentTeamId, string language)
        {
            try
            {
                return await apiService.Execute(() => teamApi.GetTeamResults(language, teamId, opponentTeamId)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetTeamResultsAsync)},
                    { "TeamId1", teamId},
                    { "TeamId2", opponentTeamId}
                };

                HandleException(ex, properties);

                return null;
            }
        }

        public async Task<IEnumerable<ITeamProfile>> GetTrendingTeams(string language)
        {
            try
            {
                return await apiService.Execute(() => teamApi.GetTrendingTeams(language));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<ITeamProfile>();
            }
        }
    }
}