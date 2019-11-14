using System;
using AutoFixture;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems;
using LiveScore.Soccer.Views.Templates.MatchDetails.Information.InfomationItems;
using NSubstitute;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    public class BaseInfoItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;
        private readonly Fixture fixture; 

        public BaseInfoItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            navigationService = baseFixture.NavigationService;
            dependencyResolver = baseFixture.DependencyResolver;
            fixture = baseFixture.CommonFixture.Specimens;
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
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.HomeScore, (byte)1)
                .With(timeline => timeline.AwayScore, (byte)2)
                .With(timeline => timeline.MatchTime, (byte)20)
                .With(timeline => timeline.Type, Enumeration.FromDisplayName<EventType>(eventType));

            // Act
            var actual = BaseItemViewModel.CreateInstance(timeline, matchInfo, navigationService, dependencyResolver);

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
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.HomeScore, (byte)1)
                .With(timeline => timeline.AwayScore, (byte)2)
                .With(timeline => timeline.MatchTime, (byte)20)
                .With(timeline => timeline.Type, Enumeration.FromDisplayName<EventType>(eventType));

            var viewModel = new BaseItemViewModel(timeline, matchInfo, navigationService, dependencyResolver);

            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedScore()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>();
            var viewModel = new BaseItemViewModel(timeline, matchInfo, navigationService, dependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal($"{timeline.HomeScore} - {timeline.AwayScore}", viewModel.Score);
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedMatchTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>().With(timeline => timeline.StoppageTime, null);
            var viewModel = new BaseItemViewModel(timeline, matchInfo, navigationService, dependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal($"{timeline.MatchTime}'", viewModel.MatchTime);
        }

        [Fact]
        public void BuildInfo_Always_BuildExpectedRowColor()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>();
            var viewModel = new BaseItemViewModel(timeline, matchInfo, navigationService, dependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal(Color.FromHex("#66FF59"), viewModel.RowColor);
        }
    }
}