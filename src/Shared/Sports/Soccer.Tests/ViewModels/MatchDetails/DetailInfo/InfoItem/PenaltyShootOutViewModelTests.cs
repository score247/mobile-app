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
    public class PenaltyShootOutViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly Fixture fixture;

        public PenaltyShootOutViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            fixture = baseFixture.CommonFixture.Specimens;

            baseFixture.DependencyResolver.Resolve<ITimelineEventImageBuilder>(Arg.Any<string>())
              .Returns(new PenaltyShootOutImageBuilder());
        }

        [Theory]
        [InlineData(true, 1, true, 1, "1 - 1")]
        [InlineData(false, 1, true, 2, "1 - 2")]
        [InlineData(true, 2, false, 2, "2 - 2")]
        [InlineData(false, 2, false, 2, "2 - 2")]
        [InlineData(true, 3, true, 3, "3 - 3")]
        public void BuildInfo_Always_ShowCorrectHomeAndAwayTeamScore(
            bool isHomeScored, byte homeScore, bool isAwayScored, byte awayScore, string expectedScore)
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.HomeShootoutPlayer, new Player { Name = "Ronaldo" })
                .With(timeline => timeline.IsHomeShootoutScored, isHomeScored)
                .With(timeline => timeline.ShootoutHomeScore, homeScore)
                .With(timeline => timeline.AwayShootoutPlayer, new Player { Name = "Messi" })
                .With(timeline => timeline.IsAwayShootoutScored, isAwayScored)
                .With(timeline => timeline.ShootoutAwayScore, awayScore);

            var viewModel = new PenaltyShootOutViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

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
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.HomeShootoutPlayer, new Player { Name = "Ronaldo" })
                .With(timeline => timeline.IsHomeShootoutScored, true)
                .With(timeline => timeline.ShootoutHomeScore, (byte)1)
                .With(timeline => timeline.AwayShootoutPlayer, null)
                .With(timeline => timeline.ShootoutAwayScore, (byte)1);
            var viewModel = new PenaltyShootOutViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("1 - 1", viewModel.Score);
            Assert.Equal(GetPenaltyGoalImage(true), viewModel.HomeImageSource);
            Assert.Equal("Ronaldo", viewModel.HomePlayerName);
            Assert.Null(viewModel.AwayImageSource);
            Assert.Null(viewModel.AwayPlayerName);
        }

        private static string GetPenaltyGoalImage(bool isScored)
            => isScored ? "images/common/ball.svg" : "images/common/missed_goal.svg";
    }
}