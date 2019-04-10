namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IMatchService
    {
        Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false);

        Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group);
    }
}
