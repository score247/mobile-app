namespace LiveScore.Features.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Domain.Models.Matches;

    public interface MatchService
    {
        Task<IEnumerable<Match>> GetMatches(int sportId, DateTime from, DateTime to, string language);
    }

    public class MatchServiceImpl : MatchService
    {
        private readonly MatchDataAccess leagueDataAccess;

        public MatchServiceImpl(MatchDataAccess leagueDataAccess)
        {
            this.leagueDataAccess = leagueDataAccess;
        }

        public async Task<IEnumerable<Match>> GetMatches(int sportId, DateTime from, DateTime to, string language)
        {
            if (sportId == 0)
            {
                return await Task.FromResult(Enumerable.Empty<Match>());
            }

            var leagues = new List<Match>();

            for (DateTime date = from.Date; date.Date < to.Date; date = date.AddDays(1))
            {
                leagues.AddRange(await leagueDataAccess.GetMatches(1, date.Date));
            }

            return leagues;
        }
    }
}