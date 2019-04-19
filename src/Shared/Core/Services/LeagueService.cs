namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Leagues;

    public interface ILeagueService
    {
        Task<IEnumerable<ILeague>> GetLeagues();
    }
}