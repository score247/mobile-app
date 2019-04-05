namespace LiveScore.Features.Leagues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LiveScore.Domain.DomainModels;

    public interface LeagueService
    {
        Task<IEnumerable<League>> GetLeagues(int sportId, DateTime from, DateTime to);
    }

    public class LeagueServiceImpl : LeagueService
    {
        //private readonly ILeagueApi leagueApi;

        //public LeagueServiceImpl(ILeagueApi leagueApi)
        //{
        //    this.leagueApi = leagueApi;
        //}

        public async Task<IEnumerable<League>> GetLeagues(int sportId, DateTime from, DateTime to)
        {
            if(sportId == 0)
            {
                return await Task.FromResult(Enumerable.Empty<League>());
            }


            return null;
        }
    }
}
