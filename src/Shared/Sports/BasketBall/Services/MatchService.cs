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
        public Task<IList<IMatch>> GetLiveMatches(int sportId, string language)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<IMatch>> GetMatches(int sportId, string language, DateRange dateRange, string timezone, bool forceFetchNewData = false)
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
