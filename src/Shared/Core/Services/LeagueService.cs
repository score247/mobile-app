using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;

namespace LiveScore.Core.Services
{
    public interface ILeagueService
    {
        Task<IEnumerable<ILeague>> GetMajorLeaguesAsync(bool forceFetchLatestData = false);
    }
}