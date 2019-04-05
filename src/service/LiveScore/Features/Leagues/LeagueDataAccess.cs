namespace LiveScore.Features.Leagues
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Domain.DomainModels;

    public interface LeagueDataAccess
    {
        Task<IEnumerable<League>> GetLeagues(int sportId, DateTime date);
    }

    public class LeagueDataAccessImpl : LeagueDataAccess
    {
        public Task<IEnumerable<League>> GetLeagues(int sportId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}