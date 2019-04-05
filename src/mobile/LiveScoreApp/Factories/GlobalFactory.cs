namespace LiveScoreApp.Factories
{
    using BasketBall.Factories;
    using Common.Services;
    using Core.Contants;
    using Core.Factories;
    using Core.Services;
    using Soccer.Factories;

    public class GlobalFactory : IGlobalFactory
    {
        private readonly IMatchApi matchApi;
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;
        private readonly ILoggingService loggingService;

        public GlobalFactory(
            IMatchApi matchApi,
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService)
        {
            this.matchApi = matchApi;
            this.leagueApi = leagueApi;
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.loggingService = loggingService;
        }

        public ISportServiceFactory BuildSportService(SportType sportType)
        {
            switch (sportType)
            {
                case SportType.Soccer:
                    return new SoccerServiceFactory(matchApi, leagueApi, settingsService, cacheService, loggingService);

                case SportType.BasketBall:
                    return new BasketBallServiceFactory();

                default:
                    return new SoccerServiceFactory(matchApi, leagueApi, settingsService, cacheService, loggingService);
            }
        }
    }
}
