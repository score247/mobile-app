using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;

namespace LiveScore.Core.Services
{
    public interface ITeamService
    {
        Task<IHeadToHeads> GetHeadToHeadsAsync(string teamId1, string teamId2, Language language, bool forceFetchLatestData = false);       
    }
}
