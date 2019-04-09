namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Leagues;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/League/GetLeagues?sportId={sportId}&language={languageCode}")]
        Task<IEnumerable<League>> GetLeagues(int sportId, string languageCode);
    }

    public interface ILeagueService
    {
        Task<IEnumerable<League>> GetLeagues();
    }
}
