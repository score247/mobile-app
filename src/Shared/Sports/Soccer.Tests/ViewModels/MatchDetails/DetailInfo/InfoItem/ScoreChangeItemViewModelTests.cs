using AutoFixture;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    public class ScoreChangeItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly Fixture fixture;

        public ScoreChangeItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            fixture = baseFixture.CommonFixture.Specimens;

            baseFixture.DependencyResolver.Resolve<ITimelineEventImageBuilder>(Arg.Any<string>())
              .Returns(new ScoreChangeImageBuilder());
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsOwnGoal_ShowOwnGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane", Method = "own_goal" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/own_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsPenaltyGoal_ShowPenaltyGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane", Method = "penalty" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/penalty_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_IsNormalGoal_ShowNormalGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane" })
                .With(timeline => timeline.Assist, new Player { Name = "Rooney" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("Rooney", viewModel.HomeAssistName);
            Assert.Equal("images/common/ball.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsOwnGoal_ShowOwnGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane", Method = "own_goal" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/own_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsPenaltyGoal_ShowPenaltyGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane", Method = "penalty" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/penalty_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_IsNormalGoal_ShowNormalGoalInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.GoalScorer, new GoalScorer { Name = "Harry Kane" })
                .With(timeline => timeline.Assist, new Player { Name = "Rooney" });

            var viewModel = new ScoreChangeItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("Rooney", viewModel.AwayAssistName);
            Assert.Equal("images/common/ball.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
        }
    }
}