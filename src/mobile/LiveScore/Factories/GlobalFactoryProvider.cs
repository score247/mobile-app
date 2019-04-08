namespace LiveScore.Factories
{
    using Core.Factories;
    using LiveScore.BasketBall.Factories;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Factories;

    public class GlobalFactoryProvider : IGlobalFactoryProvider
    {
        public GlobalFactoryProvider(
            IMatchApi matchApi,
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService)
        {
            SportServiceFactoryProvider = new SportServiceFactoryProvider();

            var soccerServiceFactory = new SoccerServiceFactory(matchApi, leagueApi, settingsService, cacheService, loggingService);
            soccerServiceFactory.RegisterTo(SportServiceFactoryProvider);

            var basketBallServiceFactory = new BasketBallServiceFactory();
            basketBallServiceFactory.RegisterTo(SportServiceFactoryProvider);
        }

        public ISportServiceFactoryProvider SportServiceFactoryProvider { get; }
    }
}
