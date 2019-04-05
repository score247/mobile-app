using System;
using BasketBall.Services;
using LiveScore.Core.Factories;
using LiveScore.Core.Services;

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
