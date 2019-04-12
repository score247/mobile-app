namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IMatchService
    {

        Task<IList<IMatch>> GetMatches(int sportId, string language, DateTime fromDate, DateTime toDate, bool forceFetchNewData = false);

        Task<IList<IMatch>> GetLiveMatches(int sportId, string language);

        Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group);
    }
}
