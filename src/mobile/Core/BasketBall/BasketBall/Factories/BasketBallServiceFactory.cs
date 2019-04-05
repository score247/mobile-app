using System;
using BasketBall.Services;
using Core.Factories;
using Core.Services;

namespace BasketBall.Factories
{
    public class BasketBallServiceFactory : ISportServiceFactory
    {
        public ILeagueService CreateLeagueService()
        {
            return new LeagueService();
        }

        public IMatchService CreateMatchService()
        {
            return new MatchService();
        }
    }
}
