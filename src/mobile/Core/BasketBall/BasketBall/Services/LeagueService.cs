namespace BasketBall.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.LeagueInfo;
    using LiveScore.Core.Services;

    public class LeagueService : ILeagueService
    {
        public Task<IList<LeagueItem>> GetLeagues()
        {
            throw new NotImplementedException();
        }
    }
}
