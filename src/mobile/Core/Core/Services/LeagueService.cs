namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models.LeagueInfo;

    public interface ILeagueService
    {
        Task<IList<LeagueItem>> GetLeaguesAsync();
    }
}
