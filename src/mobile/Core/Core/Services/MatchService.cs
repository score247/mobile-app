namespace Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using Core.Models.LeagueInfo;
    using Core.Models.MatchInfo;
    using Refit;

    public interface IMatchApi
    {
        [Get("/{sport}-t3/{group}/{lang}/schedules/{date}/results.json?api_key={key}")]
        Task<MatchSchedule> GetDailyMatches(string sport, string group, string lang, string date, string key);

        [Get("/{sport}-t3/{group}/{lang}/tournaments/{leagueId}/schedule.json?api_key={key}")]
        Task<LeagueSchedule> GetMatchesByLeague(string sport, string group, string lang, string leagueId, string key);
    }

    public interface IMatchService
    {
        Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false);

        Task<IList<Match>> GetMatchesByLeague(string leagueId, string group);
    }
}
