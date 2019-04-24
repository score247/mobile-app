namespace LiveScore.Core.Tests.Fixtures
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Soccer.Models.Teams;

    public class CommonFixture
    {
        public CommonFixture()
        {
            Comparer = new CompareLogic();
            Fixture = new Fixture();

            Fixture.Register<IMatch>(() => Fixture.Create<Match>());
            Fixture.Register<ITeam>(() => Fixture.Create<Team>());
            Fixture.Register<ITeamStatistic>(() => Fixture.Create<TeamStatistic>());
            Fixture.Register<ICoach>(() => Fixture.Create<Coach>());
            Fixture.Register<IPlayer>(() => Fixture.Create<Player>());
            Fixture.Register<ITimeLine>(() => Fixture.Create<TimeLine>());
            Fixture.Register<IMatchResult>(() => Fixture.Create<MatchResult>());
            Fixture.Register<IMatchCondition>(() => Fixture.Create<MatchCondition>());
            Fixture.Register<ILeague>(() => Fixture.Create<League>());
            Fixture.Register<IVenue>(() => Fixture.Create<Venue>());
            Fixture.Register<ILeagueCategory>(() => Fixture.Create<LeagueCategory>());
            Fixture.Register<ILeagueRound>(() => Fixture.Create<LeagueRound>());
        }

        public CompareLogic Comparer { get; }

        public Fixture Fixture { get; }
    }
}