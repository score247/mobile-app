namespace LiveScore.Soccer.Factories
{
    using LiveScore.Common.Services;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Services;

    public class SoccerServiceFactory : ISportServiceFactory
    {
        private readonly IMatchApi matchApi;
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;
        private readonly ILoggingService loggingService;
        private readonly INetworkService networkService;

        public SoccerServiceFactory(
            IMatchApi matchApi,
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService,
            INetworkService networkService)
        {
            this.matchApi = matchApi;
            this.leagueApi = leagueApi;
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.loggingService = loggingService;
            this.networkService = networkService;
        }

        public void RegisterTo(ISportServiceFactoryProvider sportServiceFactoryProvider)
        {
            sportServiceFactoryProvider.RegisterInstance(SportType.Soccer, this);
        }

        public ILeagueService CreateLeagueService()
            => new LeagueService(leagueApi, settingsService, loggingService, networkService);

        public IMatchService CreateMatchService()
            => new MatchService(matchApi, settingsService, cacheService, loggingService);
    }
}
