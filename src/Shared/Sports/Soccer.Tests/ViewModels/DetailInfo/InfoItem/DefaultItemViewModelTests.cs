//namespace Soccer.Tests.ViewModels.MatchDetailInfo
//{
//    using LiveScore.Core.Models.Matches;
//    using LiveScore.Core.Models.Teams;
//    using LiveScore.Core.Tests.Fixtures;
//    using LiveScore.Soccer.ViewModels.MatchDetailInfo;

//    using NSubstitute;

//    using Xunit;

//    public class DefaultItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
//    {
//        private readonly ITimeline timeline;
//        private readonly IMatchResult matchResult;
//        private readonly ViewModelBaseFixture baseFixture;

//        public DefaultItemViewModelTests(ViewModelBaseFixture baseFixture)
//        {
//            this.baseFixture = baseFixture;
//            timeline = Substitute.For<ITimeline>();
//            matchResult = Substitute.For<IMatchResult>();
//        }

//        [Fact]
//        public void BuildInfo_IsHomeTeamAndYellowCard_ShowHomePlayerNameAndYellowCard()
//        {
//            // Arrange
//            timeline.Team.Returns("home");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("yellow_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
//            Assert.Equal("images/common/yellow_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleHomeImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsHomeTeamAndRedYellowCard_ShowHomePlayerNameAndRedYellowCard()
//        {
//            // Arrange
//            timeline.Team.Returns("home");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("yellow_red_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
//            Assert.Equal("images/common/red_yellow_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleHomeImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsHomeTeamAndRedCard_ShowHomePlayerNameAndRedCard()
//        {
//            // Arrange
//            timeline.Team.Returns("home");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("red_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
//            Assert.Equal("images/common/red_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleHomeImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsAwayTeamAndYellowCard_ShowAwayPlayerNameAndYellowCard()
//        {
//            // Arrange
//            timeline.Team.Returns("away");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("yellow_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
//            Assert.Equal("images/common/yellow_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleAwayImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsAwayTeamAndRedYellowCard_ShowAwayPlayerNameAndRedYellowCard()
//        {
//            // Arrange
//            timeline.Team.Returns("away");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("yellow_red_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
//            Assert.Equal("images/common/red_yellow_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleAwayImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsAwayTeamAndRedCard_ShowAwayPlayerNameAndRedCard()
//        {
//            // Arrange
//            timeline.Team.Returns("away");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("red_card");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
//            Assert.Equal("images/common/red_card.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleAwayImage);
//            Assert.False(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsHomeTeam_MissPenalties_ShowExpectedInfo()
//        {
//            // Arrange
//            timeline.Team.Returns("home");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("penalty_missed");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
//            Assert.Equal("images/common/missed_penalty_goal.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleHomeImage);
//            Assert.True(viewModel.VisibleScore);
//        }

//        [Fact]
//        public void BuildInfo_IsAwayTeam_MissPenalties_ShowExpectedInfo()
//        {
//            // Arrange
//            timeline.Team.Returns("away");
//            timeline.Player.Returns(new Player { Name = "Harry Kane" });
//            timeline.Type.Returns("penalty_missed");

//            // Act
//            var viewModel = new DefaultItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
//            Assert.Equal("images/common/missed_penalty_goal.png", viewModel.ImageSource);
//            Assert.True(viewModel.VisibleAwayImage);
//            Assert.True(viewModel.VisibleScore);
//        }
//    }
//}