namespace LiveScore.Features.Leagues
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Leagues;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/League/GetLeaguesByDate?sportId={sportId}&from={fromDate}&to={toDate}")]
        Task<IEnumerable<League>> GetLeagues(int sportId, string fromDate, string toDate);
    }
}