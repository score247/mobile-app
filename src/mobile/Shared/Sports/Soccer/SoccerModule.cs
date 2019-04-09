namespace LiveScore.Soccer
{
    using AutoMapper;
    using LiveScore.Common.Services;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.DTOs.Leagues;
    using LiveScore.Soccer.DTOs.Matches;
    using LiveScore.Soccer.DTOs.Teams;
    using LiveScore.Soccer.Factories;
    using LiveScore.Soccer.Models.Leagues;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Teams;
    using LiveScore.Soccer.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;

    public class SoccerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var soccerMatchApi = containerProvider.Resolve<ISoccerMatchApi>();
            var leagueApi = containerProvider.Resolve<ILeagueApi>();
            var settingsService = containerProvider.Resolve<ISettingsService>();
            var cacheService = containerProvider.Resolve<ICacheService>();
            var loggingService = containerProvider.Resolve<ILoggingService>();
            var apiPolicy = containerProvider.Resolve<IApiPolicy>();
            var globalServiceProvider = containerProvider.Resolve<IGlobalFactoryProvider>();
            var mapper = containerProvider.Resolve<IMapper>();

            var soccerServiceFactory = new SoccerServiceFactory(soccerMatchApi, leagueApi, settingsService, cacheService, loggingService, apiPolicy, mapper);
            soccerServiceFactory.RegisterTo(globalServiceProvider.SportServiceFactoryProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatchDTO, Match>();
                cfg.CreateMap<MatchDTO, IMatch>().As<Match>();
                cfg.CreateMap<MatchResultDTO, MatchResult>();
                cfg.CreateMap<MatchResultDTO, IMatchResult>().As<MatchResult>();
                cfg.CreateMap<TimeLineDTO, TimeLine>();
                cfg.CreateMap<TimeLineDTO, ITimeLine>().As<TimeLine>();
                cfg.CreateMap<MatchConditionDTO, MatchCondition>();
                cfg.CreateMap<MatchConditionDTO, IMatchCondition>().As<MatchCondition>();
                cfg.CreateMap<VenueDTO, Venue>();
                cfg.CreateMap<VenueDTO, IVenue>().As<Venue>();

                cfg.CreateMap<LeagueDTO, League>();
                cfg.CreateMap<LeagueDTO, ILeague>().As<League>();
                cfg.CreateMap<LeagueCategoryDTO, LeagueCategory>();
                cfg.CreateMap<LeagueCategoryDTO, ILeagueCategory>().As<LeagueCategory>();
                cfg.CreateMap<LeagueRoundDTO, LeagueRound>();
                cfg.CreateMap<LeagueRoundDTO, ILeagueRound>().As<LeagueRound>();

                cfg.CreateMap<TeamDTO, Team>();
                cfg.CreateMap<TeamDTO, ITeam>().As<Team>();
                cfg.CreateMap<PlayerDTO, Player>();
                cfg.CreateMap<PlayerDTO, IPlayer>().As<Player>();
                cfg.CreateMap<CoachDTO, Coach>();
                cfg.CreateMap<CoachDTO, ICoach>().As<Coach>();
                cfg.CreateMap<TeamStatisticDTO, TeamStatistic>();
                cfg.CreateMap<TeamStatisticDTO, ITeamStatistic>().As<TeamStatistic>();
            });
            var mapper = config.CreateMapper();
            containerRegistry.RegisterInstance(mapper);
            containerRegistry.RegisterInstance(RestService.For<ISoccerMatchApi>(SettingsService.LocalEndPoint));
        }
    }
}
