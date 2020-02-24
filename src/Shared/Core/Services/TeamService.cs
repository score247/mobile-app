using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;

namespace LiveScore.Core.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<IMatch>> GetHeadToHeadsAsync(string teamId1, string teamId2, string language);

        Task<IEnumerable<IMatch>> GetTeamResultsAsync(string teamId, string opponentTeamId, string language);

        Task<IEnumerable<ITeamProfile>> GetTrendingTeams(string language);

        Task<IEnumerable<ITeamProfile>> SearchTeams(string language, string keyword);

        Task<IEnumerable<IMatch>> GetTeamMatches(string language, string teamId);
    }
}