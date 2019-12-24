using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatchesByDateAsync(DateTime dateTime, Language language);

        Task<IEnumerable<IMatch>> GetLiveMatchesAsync(Language language);

        Task<int> GetLiveMatchesCountAsync();

        Task<IEnumerable<IMatch>> GetMatchesByIds(string[] ids, Language language);
    }
}