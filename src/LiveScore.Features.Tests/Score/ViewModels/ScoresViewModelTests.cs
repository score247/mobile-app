using System;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Features.Score.ViewModels;
using Xunit;

namespace LiveScore.Features.Tests.Score.ViewModels
{
    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ScoresViewModel scoreViewModel;
        private readonly CompareLogic comparer;
        private readonly IFixture specimens;
        private readonly ViewModelBaseFixture baseFixture;

        public ScoresViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            specimens = baseFixture.CommonFixture.Specimens;
            comparer = baseFixture.CommonFixture.Comparer;

            scoreViewModel = new ScoresViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator);
        }

        [Fact]
        public void RangeOfDays_Always_GetExpectedSetValue()
        {
            // Assert
            Assert.Equal(2, scoreViewModel.RangeOfDays);
        }

        [Fact]
        public void ScoreItemSources_FirstItem_IsLiveItemViewModel()
        {
            // Arrange && Act
            var expectedLiveItemViewModel = new LiveMatchesViewModel(baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);

            // Assert
            Assert.True(comparer.Compare(expectedLiveItemViewModel, scoreViewModel.ScoreItemSources[0]).AreEqual);
        }

        [Fact]
        public void ScoreItemSources_DateItem_IsScoreItemViewModel()
        {
            // Arrange && Act
            var expectedSecondItemViewModel = new ScoreMatchesViewModel(DateTime.Today.AddDays(-2), baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);
            var expectedThirdItemViewModel = new ScoreMatchesViewModel(DateTime.Today.AddDays(-1), baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);
            var expectedTodayItemViewModel = new ScoreMatchesViewModel(DateTime.Today, baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);
            var expectedFifthItemViewModel = new ScoreMatchesViewModel(DateTime.Today.AddDays(1), baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);
            var expectedSixthItemViewModel = new ScoreMatchesViewModel(DateTime.Today.AddDays(2), baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);

            // Assert
            Assert.True(comparer.Compare(expectedSecondItemViewModel, scoreViewModel.ScoreItemSources[1]).AreEqual);
            Assert.True(comparer.Compare(expectedThirdItemViewModel, scoreViewModel.ScoreItemSources[2]).AreEqual);
            Assert.True(comparer.Compare(expectedTodayItemViewModel, scoreViewModel.ScoreItemSources[3]).AreEqual);
            Assert.True(comparer.Compare(expectedFifthItemViewModel, scoreViewModel.ScoreItemSources[4]).AreEqual);
            Assert.True(comparer.Compare(expectedSixthItemViewModel, scoreViewModel.ScoreItemSources[5]).AreEqual);
        }

        [Fact]
        public void ScoreItemSources_LastItem_IsCalendarItemViewModel()
        {
            // Arrange && Act
            var expectedCalendarItemViewModel = new CalendarMatchesViewModel(baseFixture.NavigationService, baseFixture.DependencyResolver, baseFixture.EventAggregator);

            // Assert
            Assert.True(comparer.Compare(expectedCalendarItemViewModel, scoreViewModel.ScoreItemSources[6]).AreEqual);
        }

        [Fact]
        public void OnResumeWhenNetworkOk_ViewDateIsNotToday_NavigateToHome()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}