namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Xunit;

    public class PenaltyMissedItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ITimeline timeline;
        private readonly IMatchResult matchResult;
        private readonly ViewModelBaseFixture baseFixture;

        public PenaltyMissedItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            timeline = Substitute.For<ITimeline>();
            matchResult = Substitute.For<IMatchResult>();
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_ShowHomeInfo()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });

            // Act
            var viewModel = new PenaltyMissedItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.True(viewModel.VisibleHomeMissPenaltyGoalBall);
            Assert.False(viewModel.VisibleAwayMissPenaltyGoalBall);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_ShowAwayInfo()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });

            // Act
            var viewModel = new PenaltyMissedItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.False(viewModel.VisibleHomeMissPenaltyGoalBall);
            Assert.True(viewModel.VisibleAwayMissPenaltyGoalBall);
        }
    }
}