namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Xunit;

    public class ScoreChangeItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ITimeline timeline;
        private readonly IMatchResult matchResult;
        private readonly ViewModelBaseFixture baseFixture;

        public ScoreChangeItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            timeline = Substitute.For<ITimeline>();
            matchResult = Substitute.For<IMatchResult>();
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsOwnGoal_ShowOwnGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane", Method = "own_goal" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/own_goal.png", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsPenaltyGoal_ShowPenaltyGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane", Method = "penalty" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/penalty_goal.png", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsNormalGoal_ShowNormalGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane" });
            timeline.Assist.Returns(new Player { Name = "Rooney" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("Rooney", viewModel.HomeAssistName);
            Assert.Equal("images/common/ball.png", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsOwnGoal_ShowOwnGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane", Method = "own_goal" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/own_goal.png", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsPenaltyGoal_ShowPenaltyGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane", Method = "penalty" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/penalty_goal.png", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsNormalGoal_ShowNormalGoalInfo()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.GoalScorer.Returns(new GoalScorer { Name = "Harry Kane" });
            timeline.Assist.Returns(new Player { Name = "Rooney" });

            // Act
            var viewModel = new ScoreChangeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("Rooney", viewModel.AwayAssistName);
            Assert.True(viewModel.VisibleAwayImage);
            Assert.Equal("images/common/ball.png", viewModel.ImageSource);
        }
    }
}