namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Prism.Navigation;
    using System;
    using Xunit;

    public class CardItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>s
    {
        private readonly ITimeline timeline;
        private readonly IMatchResult matchResult;
        private readonly ViewModelBaseFixture baseFixture;

        public CardItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            timeline = Substitute.For<ITimeline>();
            matchResult = Substitute.For<IMatchResult>();
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndYellowCard_ShowHomePlayerNameAndYellowCard()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("yellow_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.True(viewModel.VisibleHomeYellowCard);
            Assert.False(viewModel.VisibleAwayYellowCard);
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndRedYellowCard_ShowHomePlayerNameAndRedYellowCard()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("yellow_red_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.True(viewModel.VisibleHomeRedYellowCard);
            Assert.False(viewModel.VisibleAwayRedYellowCard);
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndRedCard_ShowHomePlayerNameAndRedCard()
        {
            // Arrange
            timeline.Team.Returns("home");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("red_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.True(viewModel.VisibleHomeRedCard);
            Assert.False(viewModel.VisibleAwayRedCard);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndYellowCard_ShowAwayPlayerNameAndYellowCard()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("yellow_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.False(viewModel.VisibleHomeYellowCard);
            Assert.True(viewModel.VisibleAwayYellowCard);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndRedYellowCard_ShowAwayPlayerNameAndRedYellowCard()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("yellow_red_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.False(viewModel.VisibleHomeRedYellowCard);
            Assert.True(viewModel.VisibleAwayRedYellowCard);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndRedCard_ShowAwayPlayerNameAndRedCard()
        {
            // Arrange
            timeline.Team.Returns("away");
            timeline.Player.Returns(new Player { Name = "Harry Kane" });
            timeline.Type.Returns("red_card");

            // Act
            var viewModel = new CardItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.False(viewModel.VisibleHomeRedCard);
            Assert.True(viewModel.VisibleAwayRedCard);
        }
    }
}