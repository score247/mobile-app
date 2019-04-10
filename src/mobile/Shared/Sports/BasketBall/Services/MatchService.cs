namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LiveScore.Basketball.Models.Leagues;
    using LiveScore.Basketball.Models.Matches;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;

    public class MatchService : IMatchService
    {
        public async Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false)
        {
            Debug.WriteLine("Call Basketball MatchService");

            var matches = new List<IMatch>
            {
                new Match { EventDate = DateTime.Today, League = new League{ Name = "Basketball" }}
            };

            return matches;
        }

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }
    }
}
