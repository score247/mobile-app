namespace LiveScore.Basketball
{
    using AutoMapper;
    using LiveScore.Basketball.DTOs.Leagues;
    using LiveScore.Basketball.DTOs.Matches;
    using LiveScore.Basketball.DTOs.Teams;
    using LiveScore.Basketball.Models.Teams;
    using LiveScore.Basketball.Services;
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Common.Configuration;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;
    using Xamarin.Forms;

    public class BasketballModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(CreateMapper());
            containerRegistry.RegisterInstance(RestService.For<IBasketballMatchApi>(Configuration.LocalEndPoint));
            containerRegistry.Register<IMatchService, MatchService>(SportType.Basketball.GetDescription());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportType.Basketball.GetDescription());
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
