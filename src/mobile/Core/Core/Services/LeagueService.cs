namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models.LeagueInfo;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/{sport}-t3/{group}/{lang}/tournaments.json?api_key={key}")]
        Task<LeagueInfo> GetLeaguesByGroup(string sport, string group, string lang, string key);
    }

    public interface ILeagueService
    {
        Task<IList<LeagueItem>> GetLeagues();
    }
}
