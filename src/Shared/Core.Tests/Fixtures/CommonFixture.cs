namespace LiveScore.Core.Tests.Fixtures
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;
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
            Specimens.Register<ITeam>(() => Specimens.Create<Team>());
            Specimens.Register<ITeamStatistic>(() => Specimens.Create<TeamStatistic>());
            Specimens.Register<ICoach>(() => Specimens.Create<Coach>());
            Specimens.Register<IPlayer>(() => Specimens.Create<Player>());
            Specimens.Register<ITimelineEvent>(() => Specimens.Create<TimelineEvent>());
            Specimens.Register<IMatchResult>(() => Specimens.Create<MatchResult>());
            Specimens.Register<ILeague>(() => Specimens.Create<League>());
            Specimens.Register<ILeagueCategory>(() => Specimens.Create<LeagueCategory>());
            Specimens.Register<ILeagueRound>(() => Specimens.Create<LeagueRound>());
            Specimens.Register<IBetTypeOdds>(() => Specimens.Create<BetTypeOdds>());
            Specimens.Register<IOddsMovement>(() => Specimens.Create<OddsMovement>());
        }

        public CompareLogic Comparer { get; }

        public Fixture Specimens { get; }
    }
}