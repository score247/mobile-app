using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface ILeagueService
    {
        Task<IEnumerable<ILeague>> GetMajorLeaguesAsync(Language language, bool forceFetchLatestData = false);

        Task<ILeagueTable> GetTable(string leagueId, string seasonId, string leagueRoundGroup, Language language);

        Task<IEnumerable<IMatch>> GetFixtures(string leagueId, Language language);
    }
}