namespace Scores.Tests.ViewModels
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>, IDisposable
    {
        private readonly ScoresViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly IList<IMatch> matchData;
        private readonly CompareLogic comparer;

        public ScoresViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            comparer = viewModelBaseFixture.CommonFixture.Comparer;

            matchData = viewModelBaseFixture.CommonFixture.Fixture
                .CreateMany<IMatch>().ToList();

            matchService = Substitute.For<IMatchService>();

            viewModelBaseFixture.DepdendencyResolver
               .Resolve<IMatchService>(Arg.Any<string>())
               .Returns(matchService);

            viewModel = new ScoresViewModel(
                viewModelBaseFixture.NavigationService,
                viewModelBaseFixture.DepdendencyResolver,
                viewModelBaseFixture.EventAggregator);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            viewModel.Dispose();
        }

        [Fact]
        public void IsLoading_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsLoading = true;

            // Act
            var actual = viewModel.IsLoading;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsNotLoading_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsLoading = true;

            // Act
            var actual = viewModel.IsNotLoading;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsRefreshing_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsRefreshing = true;

            // Act
            var actual = viewModel.IsRefreshing;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task RefreshCommand_OnExecuting_RefreshMatchListItemSourceData()
        {
            // Arrange
            matchService.GetMatches(viewModel.SettingsService.UserSettings, Arg.Any<DateRange>(), true).Returns(matchData);

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnNavigatingTo_MatchItemSourceIsNull_LoadDataFromService()
        {
            // Arrange
            matchService.GetMatches(
                viewModel.SettingsService.UserSettings,
                Arg.Is<DateRange>(dr => dr.FromDate == DateTime.Today.AddDays(-1) && dr.ToDate == DateTime.Today.EndOfDay()),
                false).Returns(matchData);

            // Act
            viewModel.OnNavigatingTo(null);

            // Assert
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnPublishingDateBarItemSelectedEvent_Always_LoadDataByDateRange()
        {
            // Arrange
            viewModel.MatchItemSource = new ObservableCollection<IGrouping<dynamic, IMatch>>(matchData.GroupBy(match => match));
            matchService.GetMatches(
                  viewModel.SettingsService.UserSettings,
                  Arg.Is<DateRange>(dr => dr.FromDate == DateTime.Today.AddDays(-1) && dr.ToDate == DateTime.Today.EndOfDay()),
                  false).Returns(matchData);
            viewModel.OnNavigatingTo(null);

            // Act
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());

            // Assert
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnDisappearing_PublishEvent_NotCallMatchServiceToGetMatches()
        {
            // Arrange
            viewModel.MatchItemSource = new ObservableCollection<IGrouping<dynamic, IMatch>>(matchData.GroupBy(match => match));
            viewModel.OnNavigatingTo(null);

            // Act
            viewModel.OnDisappearing();
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());

            // Assert
            matchService.DidNotReceive().GetMatches(Arg.Any<UserSettings>(), Arg.Any<DateRange>(), Arg.Any<bool>());
        }

        [Fact]
        public void OnResume_SelectedDateIsNotToday_NavigateToHome()
        {
            // Arrange
            viewModel.SelectedDate = DateTime.Today.AddDays(1);

            // Act
            viewModel.OnResume();

            // Assert
            viewModel.NavigationService.Received(1).NavigateAsync("app:///MainView/MenuTabbedView");
        }
    }
}