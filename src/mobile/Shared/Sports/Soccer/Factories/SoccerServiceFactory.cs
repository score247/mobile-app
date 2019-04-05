namespace LiveScore.Soccer.Factories
{
    using LiveScore.Common.Services;
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

        public SoccerServiceFactory(
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

        public ILeagueService CreateLeagueService()
            => new LeagueService(leagueApi, settingsService, loggingService);

        public IMatchService CreateMatchService()
            => new MatchService(matchApi, settingsService, cacheService, loggingService);
    }
}
