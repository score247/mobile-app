namespace LiveScore.Factories
{
    using Core.Factories;
    using LiveScore.BasketBall.Factories;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Factories;
    using Prism.Ioc;

    public class GlobalFactoryProvider : IGlobalFactoryProvider
    {
        private readonly IContainerProvider container;

        public GlobalFactoryProvider(IContainerProvider container)
        {
            this.container = container;
            RegisterSportServiceFactories();
        }

        public ISportServiceFactoryProvider SportServiceFactoryProvider { get; private set; }

        private void RegisterSportServiceFactories()
        {
            var matchApi = container.Resolve<IMatchApi>();
            var leagueApi = container.Resolve<ILeagueApi>();
            var settingsService = container.Resolve<ISettingsService>();
            var cacheService = container.Resolve<ICacheService>();
            var loggingService = container.Resolve<ILoggingService>();

            SportServiceFactoryProvider = new SportServiceFactoryProvider();

            var soccerServiceFactory = new SoccerServiceFactory(matchApi, leagueApi, settingsService, cacheService, loggingService, networkService);
            soccerServiceFactory.RegisterTo(SportServiceFactoryProvider);

            var basketBallServiceFactory = new BasketBallServiceFactory();
            basketBallServiceFactory.RegisterTo(SportServiceFactoryProvider);
        }
    }
}
