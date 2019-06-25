namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Xunit;

    public class PenaltyShootOutViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly Timeline timeline;
        private readonly IMatchResult matchResult;
        private readonly ViewModelBaseFixture baseFixture;

        public PenaltyShootOutViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            timeline = new Timeline();
            matchResult = Substitute.For<IMatchResult>();
        }

        [Theory]
        [InlineData(true, 1, true, 1, "1 - 1")]
        [InlineData(false, 1, true, 2, "1 - 2")]
        [InlineData(true, 2, false, 2, "2 - 2")]
        [InlineData(false, 2, false, 2, "2 - 2")]
        [InlineData(true, 3, true, 3, "3 - 3")]
        public void BuildInfo_Always_ShowCorrectHomeAndAwayTeamScore(
            bool isHomeScored, int homeScore, bool isAwayScored, int awayScore, string expectedScore)
        {
            // Arrange
            timeline.HomeShootoutPlayer = new Player { Name = "Ronaldo" };
            timeline.IsHomeShootoutScored = isHomeScored;
            timeline.ShootoutHomeScore = homeScore;
            timeline.AwayShootoutPlayer = new Player { Name = "Messi" };
            timeline.IsAwayShootoutScored = isAwayScored;
            timeline.ShootoutAwayScore = awayScore;

            // Act
            var viewModel = new PenaltyShootOutViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal(expectedScore, viewModel.Score);
            Assert.Equal(GetPenaltyGoalImage(isHomeScored), viewModel.HomeImageSource);
            Assert.Equal("Ronaldo", viewModel.HomePlayerName);
            Assert.Equal(GetPenaltyGoalImage(isAwayScored), viewModel.AwayImageSource);
            Assert.Equal("Messi", viewModel.AwayPlayerName);
        }

        [Fact]
        public void BuildInfo_HomeShootFirst_ShowHomeShoot()
        {
            // Arrange
            timeline.HomeShootoutPlayer = new Player { Name = "Ronaldo" };
            timeline.IsHomeShootoutScored = true;
            timeline.ShootoutHomeScore = 1;
            timeline.AwayShootoutPlayer = null;

            // Act
            var viewModel = new PenaltyShootOutViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("1 - 0", viewModel.Score);
            Assert.Equal(GetPenaltyGoalImage(true), viewModel.HomeImageSource);
            Assert.Equal("Ronaldo", viewModel.HomePlayerName);
            Assert.Null(viewModel.AwayImageSource);
            Assert.Null(viewModel.AwayPlayerName);
        }

        private static string GetPenaltyGoalImage(bool isScored)
            => isScored ? "images/common/penalty_goal.png" : "images/common/missed_penalty_goal.png";
    }
}