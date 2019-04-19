namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;

    public class BasketballLeagueService : ILeagueService
    {
        public Task<IEnumerable<ILeague>> GetLeagues()
        {
            throw new NotImplementedException();
        }
    }
}
