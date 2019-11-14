using AutoFixture;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    public class DefaultItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly Fixture fixture;

        public DefaultItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            fixture = baseFixture.CommonFixture.Specimens;

            baseFixture.DependencyResolver.Resolve<ITimelineEventImageBuilder>(Arg.Any<string>())
               .Returns(new DefaultEventImageBuilder());
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndYellowCard_ShowHomePlayerNameAndYellowCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.YellowCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/yellow_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndYellowRedCard_ShowHomePlayerNameAndYellowRedCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.YellowRedCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/red_yellow_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsHomeTeamAndRedCard_ShowHomePlayerNameAndRedCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.RedCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/red_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndYellowCard_ShowAwayPlayerNameAndYellowCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.YellowCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/yellow_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndYellowRedCard_ShowHomePlayerNameAndYellowRedCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.YellowRedCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/red_yellow_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsAwayTeamAndRedCard_ShowHomePlayerNameAndRedCard()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.RedCard);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/red_card.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
            Assert.False(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsHomeTeam_PenaltyMissed_ShowExpectedInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "home")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.PenaltyMissed);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.HomePlayerName);
            Assert.Equal("images/common/missed_penalty_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleHomeImage);
            Assert.True(viewModel.VisibleScore);
        }

        [Fact]
        public void BuildInfo_IsAwayTeam_PenaltyMissed_ShowExpectedInfo()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Team, "away")
                .With(timeline => timeline.Player, new Player { Name = "Harry Kane" })
                .With(timeline => timeline.Type, EventType.PenaltyMissed);
            var viewModel = new DefaultItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Harry Kane", viewModel.AwayPlayerName);
            Assert.Equal("images/common/missed_penalty_goal.svg", viewModel.ImageSource);
            Assert.True(viewModel.VisibleAwayImage);
            Assert.True(viewModel.VisibleScore);
        }
    }
}