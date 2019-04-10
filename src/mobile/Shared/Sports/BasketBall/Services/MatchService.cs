namespace LiveScore.BasketBall.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LiveScore.BasketBall.Models.Leagues;
    using LiveScore.BasketBall.Models.Matches;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;

    public class MatchService : IMatchService
    {
        public async Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false)
        {
            Debug.WriteLine("Call BasketBall MatchService");

            var matches = new List<IMatch>
            {
                new Match { EventDate = DateTime.Today, League = new League{ Name = "BasketBall" }}
            };

            return matches;
        }

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }
    }
}
