namespace LiveScore.BasketBall.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;

    public class MatchService : IMatchService
    {
        public Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Match>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }
    }
}
