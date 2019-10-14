using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool getLatestData = false);

        Task<IEnumerable<IMatch>> GetLiveMatches(Language language, bool getLatestData = false);
    }
}