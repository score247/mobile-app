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
                cfg.CreateMap<MatchDto, Match>();
                cfg.CreateMap<MatchDto, IMatch>().As<Match>();
                cfg.CreateMap<MatchResultDto, MatchResult>();
                cfg.CreateMap<MatchResultDto, IMatchResult>().As<MatchResult>();
                cfg.CreateMap<TimeLineDto, TimeLine>();
                cfg.CreateMap<TimeLineDto, ITimeLine>().As<TimeLine>();
                cfg.CreateMap<MatchConditionDto, MatchCondition>();
                cfg.CreateMap<MatchConditionDto, IMatchCondition>().As<MatchCondition>();
                cfg.CreateMap<VenueDto, Venue>();
                cfg.CreateMap<VenueDto, IVenue>().As<Venue>();

                cfg.CreateMap<LeagueDto, League>();
                cfg.CreateMap<LeagueDto, ILeague>().As<League>();
                cfg.CreateMap<LeagueCategoryDto, LeagueCategory>();
                cfg.CreateMap<LeagueCategoryDto, ILeagueCategory>().As<LeagueCategory>();
                cfg.CreateMap<LeagueRoundDto, LeagueRound>();
                cfg.CreateMap<LeagueRoundDto, ILeagueRound>().As<LeagueRound>();

                cfg.CreateMap<TeamDto, Team>();
                cfg.CreateMap<TeamDto, ITeam>().As<Team>();
                cfg.CreateMap<PlayerDto, Player>();
                cfg.CreateMap<PlayerDto, IPlayer>().As<Player>();
                cfg.CreateMap<CoachDto, Coach>();
                cfg.CreateMap<CoachDto, ICoach>().As<Coach>();
                cfg.CreateMap<TeamStatisticDto, TeamStatistic>();
                cfg.CreateMap<TeamStatisticDto, ITeamStatistic>().As<TeamStatistic>();
            });

            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}