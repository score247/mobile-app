namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Enumerations;
    using Models.Matches;

    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool getLatestData = false);

        Task<IEnumerable<IMatch>> GetLiveMatches(Language language, bool getLatestData = false);

        Task<byte> GetLiveMatchCount(Language language, bool getLatestData = false);
    }
}