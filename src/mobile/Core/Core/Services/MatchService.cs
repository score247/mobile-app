namespace Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models.MatchInfo;

    public interface IMatchService
    {
        Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false);

        Task<IList<Match>> GetMatchesByLeague(string leagueId, string group);
    }
}
