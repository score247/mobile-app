namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Enumerations;
    using Models.Matches;

    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool forceFetchNewData = false);

        Task<IMatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false);
    }
}