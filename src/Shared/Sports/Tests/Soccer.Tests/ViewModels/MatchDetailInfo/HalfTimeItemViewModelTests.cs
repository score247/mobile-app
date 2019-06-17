namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using Xunit;

    public class HalfTimeItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ITimeline timeline;
        private readonly IMatchResult matchResult;
        private readonly ViewModelBaseFixture baseFixture;

        public HalfTimeItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            timeline = Substitute.For<ITimeline>();
            matchResult = Substitute.For<IMatchResult>();
        }

        [Fact]
        public void BuildInfo_Always_SetRowColor()
        {
            // Act
            var viewModel = new HalfTimeItemViewModel(timeline, null, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal(Color.FromHex("#1D2133"), viewModel.RowColor);
        }

        [Fact]
        public void BuildInfo_Always_SetMainEventStatus()
        {
            // Act
            var viewModel = new HalfTimeItemViewModel(timeline, null, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("Half Time", viewModel.MainEventStatus);
        }

        [Fact]
        public void BuildInfo_HasMatchPeriodResult_ShowScore()
        {
            // Arrange
            matchResult.MatchPeriods.Returns(new List<MatchPeriod>
            {
                new MatchPeriod { HomeScore = 1, AwayScore = 2 }
            });

            // Act
            var viewModel = new HalfTimeItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("1 - 2", viewModel.Score);
        }

        [Fact]
        public void BuildInfo_NoMatchPeriodResult_ShowHyphen()
        {
            // Act
            var viewModel = new HalfTimeItemViewModel(timeline, null, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Assert
            Assert.Equal("-", viewModel.Score);
        }
    }
}