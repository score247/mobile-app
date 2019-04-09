namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;
    using Refit;

    public interface IMatchApi
    {
        [Get("/{sport}-t3/{group}/{lang}/schedules/{date}/results.json?api_key={key}")]
        Task<IMatch> GetDailyMatches(string sport, string group, string lang, string date, string key);
    }

    public interface IMatchService
    {
        Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false);

        Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group);
    }
}
