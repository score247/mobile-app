namespace LiveScore.BasketBall.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;

    public class LeagueService : ILeagueService
    {
        public Task<IEnumerable<League>> GetLeagues()
        {
            throw new NotImplementedException();
        }

        public Task<IList<LeagueItem>> GetLeaguesAndRetry()
        {
            throw new NotImplementedException();
        }
    }
}
