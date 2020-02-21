namespace LiveScore.Core.Tests.Fixtures
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Leagues;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Teams;

    public class CommonFixture
    {
        public CommonFixture()
        {
            Comparer = new CompareLogic();
            Specimens = new Fixture();
            Specimens.Register<IMatch>(() => Specimens.Create<SoccerMatch>());
            Specimens.Register((System.Func<ITeamStatistic>)(() => Specimens.Create<Soccer.Models.Teams.TeamStatistic>()));
            Specimens.Register<ICoach>(() => Specimens.Create<Coach>());
            Specimens.Register<IPlayer>(() => Specimens.Create<Player>());
            Specimens.Register<ITimelineEvent>(() => Specimens.Create<TimelineEvent>());
            Specimens.Register<IMatchResult>(() => Specimens.Create<MatchResult>());
            Specimens.Register<ILeague>(() => Specimens.Create<League>());
            Specimens.Register<ILeagueCategory>(() => Specimens.Create<LeagueCategory>());
            Specimens.Register<ILeagueRound>(() => Specimens.Create<LeagueRound>());
        }

        public CompareLogic Comparer { get; }

        public Fixture Specimens { get; }
    }
}