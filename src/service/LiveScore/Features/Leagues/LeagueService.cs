namespace LiveScore.Features.Leagues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Leagues;

    public interface LeagueService
    {
        Task<IEnumerable<League>> GetLeagues(int sportId, DateTime from, DateTime to);
    }

    public class LeagueServiceImpl : LeagueService
    {
        private readonly LeagueDataAccess leagueDataAccess;

        public LeagueServiceImpl(LeagueDataAccess leagueDataAccess)
        {
            this.leagueDataAccess = leagueDataAccess;
        }

        public async Task<IEnumerable<League>> GetLeagues(int sportId, DateTime from, DateTime to)
        {
            if (sportId == 0)
            {
                return await Task.FromResult(Enumerable.Empty<League>());
            }

            var leagues = new List<League>();

            for (DateTime date = from.Date; date.Date < to.Date; date = date.AddDays(1))
            {
                leagues.AddRange(await leagueDataAccess.GetLeagues(1, date.Date));
            }

            return leagues;
        }
    }
}