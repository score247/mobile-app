namespace LiveScore.Soccer
{
    using AutoMapper;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Configuration;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.DTOs.Leagues;
    using LiveScore.Soccer.DTOs.Matches;
    using LiveScore.Soccer.DTOs.Teams;
    using LiveScore.Soccer.Models.Teams;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.Views.Templates;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;
    using Xamarin.Forms;

    public class SoccerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(CreateMapper());
            containerRegistry.RegisterInstance(RestService.For<ISoccerMatchApi>(Configuration.LocalEndPoint));
            containerRegistry.RegisterInstance(RestService.For<ILeagueApi>(Configuration.LocalEndPoint));
            containerRegistry.Register<IMatchService, MatchService>(SportType.Soccer.GetDescription());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportType.Soccer.GetDescription());
        }

        private static IMapper CreateMapper()
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

            return mapper;
        }
    }
}