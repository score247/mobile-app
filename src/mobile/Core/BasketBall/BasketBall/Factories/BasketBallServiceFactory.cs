namespace LiveScore.BasketBall.Factories
{
    using LiveScore.BasketBall.Services;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;

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
