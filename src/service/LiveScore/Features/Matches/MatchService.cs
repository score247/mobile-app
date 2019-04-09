﻿namespace LiveScore.Features.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Features.Matches.Models;

    public interface MatchService
    {
        Task<IEnumerable<Match>> GetMatches(int sportId, DateTime from, DateTime to, string language);
    }

    public class MatchServiceImpl : MatchService
    {
        private readonly MatchDataAccess matchDataAccess;

        public MatchServiceImpl(MatchDataAccess matchDataAccess)
        {
            this.matchDataAccess = matchDataAccess;
        }

        public async Task<IEnumerable<Match>> GetMatches(
            int sportId,
            DateTime from,
            DateTime to,
            string language)
        {
            if (sportId <= 0
                || from > to
                || from == DateTime.MinValue
                || to == DateTime.MinValue)
            {
                return await Task.FromResult(Enumerable.Empty<Match>());
            }

            var matches = new List<Match>();

            const int dayIncrementIndex = 1;
            for (var date = from.Date; date.Date < to.Date; date = date.AddDays(dayIncrementIndex))
            {
                matches.AddRange(await matchDataAccess.GetMatches(sportId, date.Date, language));
            }

            return matches;
        }
    }
}