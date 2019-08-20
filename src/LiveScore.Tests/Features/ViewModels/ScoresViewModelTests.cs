namespace Scores.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using LiveScore.Soccer.Models.Matches;
    using NSubstitute;
    using Xunit;

    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ScoresViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly IHubService hubService;
        private readonly IList<IMatch> matchData;
        private readonly CompareLogic comparer;
        private readonly Fixture specimens;
        private readonly FakeHubConnection hubConnection;

        public ScoresViewModelTests(ViewModelBaseFixture baseFixture)
        {
            specimens = baseFixture.CommonFixture.Specimens;
            comparer = baseFixture.CommonFixture.Comparer;
            matchData = baseFixture.CommonFixture.Specimens
                .CreateMany<IMatch>().ToList();

            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver
               .Resolve<IMatchService>("1")
               .Returns(matchService);

            hubConnection = Substitute.For<FakeHubConnection>();
            hubService = Substitute.For<IHubService>();
            hubService.BuildMatchEventHubConnection().Returns(hubConnection);
            baseFixture.DependencyResolver
                .Resolve<IHubService>("1")
                .Returns(hubService);

            viewModel = new ScoresViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator);
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
        public void RefreshCommand_OnExecuting_RefreshMatchListItemSourceData()
        {
            // Arrange
            matchService.GetMatches(Arg.Any<DateRange>(), Language.English, true).Returns(matchData);

            // Act
            viewModel.RefreshCommand.Execute();

            // Assert
            var actualMatchData = viewModel.MatchItemsSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void TappedMatchCommand_OnExecuting_CallNavigationService()
        {
            // Arrange
            matchService.GetMatches(Arg.Any<DateRange>(), Language.English, true).Returns(matchData);
            viewModel.RefreshCommand.Execute();
            var matchViewModel = viewModel.MatchItemsSource.SelectMany(group => group).FirstOrDefault();

            // Act
            viewModel.TappedMatchCommand.Execute(matchViewModel);

            // Assert
            var navService = viewModel.NavigationService as FakeNavigationService;
            Assert.Equal("MatchDetailView" + viewModel.SettingsService.CurrentSportType.Value, navService.NavigationPath);
            Assert.Equal(matchViewModel.Match, navService.Parameters["Match"]);
        }

        [Fact]
        public void ClickSearchCommand_OnExecuting_CallNavigationService()
        {
            // Act
            viewModel.ClickSearchCommand.Execute();

            // Assert
            var navService = viewModel.NavigationService as FakeNavigationService;
            Assert.Equal("SearchNavigationPage/SearchView", navService.NavigationPath);
            Assert.True(navService.UseModalNavigation);
        }

        [Fact]
        public void OnNavigatedTo_MatchItemSourceIsNull_LoadDataFromService()
        {
            // Arrange
            matchService.GetMatches(
                Arg.Is<DateRange>(dr => dr.From == DateTime.Today.AddDays(-1) && dr.To == DateTime.Today.EndOfDay()),
                Language.English,
                false).Returns(matchData);

            // Act
            viewModel.OnNavigatedTo(null);

            // Assert
            var actualMatchData = viewModel.MatchItemsSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnNavigatedTo_MatchItemSourceIsNotNull_LoadDataFromService()
        {
            // Arrange
            viewModel.OnNavigatedTo(null);
            matchService.GetMatches(
                Arg.Any<DateRange>(),
                Language.English,
                true).Returns(matchData);

            // Act
            viewModel.OnNavigatedTo(null);

            // Assert
            var actualMatchData = viewModel.MatchItemsSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnAppearing_PublishDateBarItemSelectedEvent_LoadDataByDateRange()
        {
            // Arrange
            matchService.GetMatches(
                  Arg.Is<DateRange>(dr => dr.From == DateTime.Today.AddDays(-1) && dr.To == DateTime.Today.EndOfDay()),
                  Language.English,
                  false).Returns(matchData);
            viewModel.OnAppearing();

            // Act
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());

            // Assert
            var actualMatchData = viewModel.MatchItemsSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnAppearing_Always_CallMatchServiceToSubscribeChanged()
        {
            // Act
            viewModel.OnAppearing();

            // Assert
            matchService.Received(1)
                .SubscribeMatchEvent(hubConnection, Arg.Any<Action<byte, IMatchEvent>>());
        }

        [Fact]
        public void OnDisappearing_PublishEvent_NotCallMatchServiceToGetMatches()
        {
            // Arrange
            viewModel.OnAppearing();

            // Act
            viewModel.OnDisappearing();
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());

            // Assert
            matchService.DidNotReceive().GetMatches(Arg.Any<DateRange>(), Arg.Any<Language>(), Arg.Any<bool>());
        }

        [Fact]
        public void OnResume_Always_CallMatchServiceToSubscribeChanged()
        {
            // Act
            viewModel.OnResume();

            // Assert
            matchService.Received(1)
                  .SubscribeMatchEvent(hubConnection, Arg.Any<Action<byte, IMatchEvent>>());
        }

        [Fact]
        public void OnResume_SelectedDateIsNotToday_NavigateToHome()
        {
            // Arrange
            viewModel.SelectedDate = DateTime.Today.AddDays(10);
            var navigationService = viewModel.NavigationService as FakeNavigationService;

            // Act
            viewModel.OnResume();

            // Assert
            Assert.Equal("app:///MainView/MenuTabbedView", navigationService.NavigationPath);
        }

        [Fact]
        public void OnMatchChanged_SportIdIsCurrent_ChangeMatchData()
        {
            // Arrange
            const byte sportId = 1;
            InitViewModelData(out IMatchEvent matchEvent);

            // Act
            viewModel.OnMatchesChanged(sportId, matchEvent);

            // Assert
            var expectedMatch = viewModel.MatchItemsSource
                .SelectMany(g => g)
                .FirstOrDefault(m => m.Match.Id == matchEvent.MatchId)?.Match;

            Assert.Equal(expectedMatch.MatchResult, matchEvent.MatchResult);
            Assert.Equal(expectedMatch.LatestTimeline, matchEvent.Timeline);
        }

        [Fact]
        public void OnMatchChanged_SportIdIsNotCurrent_NotChangeMatchData()
        {
            // Arrange
            const byte sportId = 2;
            InitViewModelData(out IMatchEvent matchEvent);

            // Act
            viewModel.OnMatchesChanged(sportId, matchEvent);

            // Assert
            var expectedMatch = viewModel.MatchItemsSource
                .SelectMany(g => g)
                .FirstOrDefault(m => m.Match.Id == matchEvent.MatchId)?.Match;

            Assert.NotEqual(expectedMatch?.MatchResult, matchEvent.MatchResult);
            Assert.NotEqual(expectedMatch?.LatestTimeline, matchEvent.Timeline);
        }

        private void InitViewModelData(out IMatchEvent matchEvent)
        {
            matchService.GetMatches(Arg.Any<DateRange>(), Language.English, false).Returns(matchData);
            matchEvent = new MatchEvent(matchData.FirstOrDefault().Id, specimens.Create<MatchResult>(), specimens.Create<TimelineEvent>());

            viewModel.OnNavigatedTo(null);
        }
    }
}