using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<IMatch>> GetHeadToHeadsAsync(string teamId1, string teamId2, string language);       
    }
}
