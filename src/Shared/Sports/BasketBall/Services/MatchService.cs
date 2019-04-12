namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;

    public class MatchService : IMatchService
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false)
        {
            Debug.WriteLine("Call Basketball MatchService");

            var matches = new List<IMatch>
            {
                new Match { EventDate = DateTime.Today, League = new League{ Name = "Basketball" }}
            };

            return matches;
        }

        public Task<IList<IMatch>> GetLiveMatches(int sportId, string languge)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IMatch>> GetMatches(int sportId, string languge, DateTime fromDate, DateTime toDate, bool forceFetchNewData = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IMatch>> GetMatches(int sportId, string language, DateRange dateRange, bool forceFetchNewData = false)
        {
            throw new NotImplementedException();
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }
    }
}
