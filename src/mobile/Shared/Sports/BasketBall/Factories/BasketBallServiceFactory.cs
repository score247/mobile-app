namespace LiveScore.Basketball.Factories
{
    using LiveScore.Basketball.Services;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;

    public class BasketballServiceFactory : ISportServiceFactory
    {

        public ILeagueService CreateLeagueService()
        {
            return new LeagueService();
        }

        public IMatchService CreateMatchService()
        {
            return new MatchService();
        }

        public void RegisterTo(ISportServiceFactoryProvider sportServiceFactoryProvider)
        {
            sportServiceFactoryProvider.RegisterInstance(SportType.Basketball, this);
        }
    }
}
