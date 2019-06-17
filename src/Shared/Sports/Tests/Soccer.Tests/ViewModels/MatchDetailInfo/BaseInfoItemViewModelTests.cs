namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using System;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using LiveScore.Soccer.Views.Templates.MatchDetailInfo;
    using NSubstitute;
    using Xamarin.Forms;
    using Xunit;

    public class BaseInfoItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ITimeline timeline;
        private readonly IMatchResult matchResult;
        private readonly BaseInfoItemViewModel viewModel;

        public BaseInfoItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            timeline = Substitute.For<ITimeline>();
            matchResult = Substitute.For<IMatchResult>();

            timeline.HomeScore.Returns(1);
            timeline.AwayScore.Returns(2);
            timeline.MatchTime.Returns(20);
            viewModel = new BaseInfoItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);
        }

        [Theory]
        [InlineData("score_change", typeof(ScoreChangeItemViewModel))]
        [InlineData("yellow_card", typeof(CardItemViewModel))]
        [InlineData("yellow_red_card", typeof(CardItemViewModel))]
        [InlineData("red_card", typeof(CardItemViewModel))]
        [InlineData("break_start", typeof(HalfTimeItemViewModel))]
        [InlineData("penalty_missed", typeof(PenaltyMissedItemViewModel))]
        [InlineData("corner_kick", typeof(BaseInfoItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(string eventType, Type expectedType)
        {
            // Arrange
            timeline.Type.Returns(eventType);

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData("score_change", typeof(ScoreChangeItemTemplate))]
        [InlineData("yellow_card", typeof(CardItemTemplate))]
        [InlineData("yellow_red_card", typeof(CardItemTemplate))]
        [InlineData("red_card", typeof(CardItemTemplate))]
        [InlineData("break_start", typeof(EventStatusItemTemplate))]
        [InlineData("penalty_missed", typeof(PenaltyMissedItemTemplate))]
        [InlineData("corner_kick", typeof(EventStatusItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(string eventType, Type expectedType)
        {
            // Arrange
            timeline.Type.Returns(eventType);

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