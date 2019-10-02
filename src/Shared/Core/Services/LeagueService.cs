namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Leagues;

    public interface ILeagueService
    {
        Task<IEnumerable<ILeague>> GetMajorLeagues(bool getLatestData = false);
    }
}