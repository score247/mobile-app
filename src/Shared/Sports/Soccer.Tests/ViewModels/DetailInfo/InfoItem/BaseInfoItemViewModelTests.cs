namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using LiveScore.Soccer.Views.Templates.MatchDetailInfo;
    using NSubstitute;
    using Xamarin.Forms;
    using Xunit;

    public class BaseInfoItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ITimelineEvent timeline;
        private readonly IMatchResult matchResult;
        private readonly BaseItemViewModel viewModel;

        public BaseInfoItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            timeline = Substitute.For<ITimelineEvent>();
            matchResult = Substitute.For<IMatchResult>();

            timeline.HomeScore.Returns(1);
            timeline.AwayScore.Returns(2);
            timeline.MatchTime.Returns(20);
            viewModel = new BaseItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);
        }

        [Theory]
        [InlineData("score_change", typeof(ScoreChangeItemViewModel))]
        [InlineData("yellow_card", typeof(DefaultItemViewModel))]
        [InlineData("yellow_red_card", typeof(DefaultItemViewModel))]
        [InlineData("red_card", typeof(DefaultItemViewModel))]
        [InlineData("penalty_missed", typeof(DefaultItemViewModel))]
        [InlineData("break_start", typeof(MainEventItemViewModel))]
        [InlineData("match_ended", typeof(MainEventItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(string eventType, Type expectedType)
        {
            // Arrange
            timeline.Type.Returns(Enumeration.FromDisplayName<EventType>(eventType));

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData("score_change", typeof(ScoreChangeItemTemplate))]
        [InlineData("yellow_card", typeof(DefaultItemTemplate))]
        [InlineData("yellow_red_card", typeof(DefaultItemTemplate))]
        [InlineData("red_card", typeof(DefaultItemTemplate))]
        [InlineData("penalty_missed", typeof(DefaultItemTemplate))]
        [InlineData("break_start", typeof(MainEventItemTemplate))]
        [InlineData("match_ended", typeof(MainEventItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(string eventType, Type expectedType)
        {
            // Arrange
            timeline.Type.Returns(Enumeration.FromDisplayName<EventType>(eventType));
            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedScore()
        {
            // Act
            var actualScore = viewModel.Score;

            // Assert
            Assert.Equal("1 - 2", actualScore);
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedMatchTime()
        {
            // Act
            var actualMatchTime = viewModel.MatchTime;

            // Assert
            Assert.Equal("20'", actualMatchTime);
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedRowColor()
        {
            // Act
            var actualRowColor = viewModel.RowColor;

            // Assert
            Assert.Equal(Color.FromHex("#1D2133"), actualRowColor);
        }
    }
}