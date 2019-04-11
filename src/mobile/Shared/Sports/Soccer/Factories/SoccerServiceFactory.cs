namespace LiveScore.Soccer.Factories
{
    using AutoMapper;
    using LiveScore.Common.Services;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Services;

    public class SoccerServiceFactory : IServiceFactory
    {
        private readonly ISoccerMatchApi soccerMatchApi;
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;
        private readonly ILoggingService loggingService;
        private readonly IApiPolicy networkService;
        private readonly IMapper mapper;

        public SoccerServiceFactory(
            ISoccerMatchApi soccerMatchApi,
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService,
            IApiPolicy networkService,
            IMapper mapper)
        {
            this.soccerMatchApi = soccerMatchApi;
            this.leagueApi = leagueApi;
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.loggingService = loggingService;
            this.networkService = networkService;
            this.mapper = mapper;
        }

        public ILeagueService CreateLeagueService()
            => new LeagueService(leagueApi, settingsService, loggingService, networkService);

        public IMatchService CreateMatchService()
            => new MatchService(soccerMatchApi, settingsService, cacheService, loggingService, mapper);
    }
}
