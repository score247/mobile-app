namespace Scores.Tests.ViewModels
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Configuration;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Controls.DateBar.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using LiveScore.Score.ViewModels;
    using Microsoft.AspNetCore.SignalR.Client;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly ScoresViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly IList<IMatch> matchData;
        private readonly CompareLogic comparer;
        private readonly Fixture specimens;
        private readonly IEnumerable<MatchItemSourceViewModel> matchItemViewModels;
        private readonly FakeHubConnection hubConnection;

        public ScoresViewModelTests(ViewModelBaseFixture baseFixture)
        {
            specimens = baseFixture.CommonFixture.Specimens;
            comparer = baseFixture.CommonFixture.Comparer;
            matchData = baseFixture.CommonFixture.Specimens
                .CreateMany<IMatch>().ToList();
            matchService = Substitute.For<IMatchService>();

            baseFixture.DepdendencyResolver
               .Resolve<IMatchService>(Arg.Any<string>())
               .Returns(matchService);

            hubConnection = Substitute.For<FakeHubConnection>();
            baseFixture.HubConnectionBuilder
                .WithUrl($"{Configuration.LocalHubEndPoint}/MatchHub")
                .Build()
                .Returns(hubConnection);

            matchItemViewModels = matchData.Select(match => new MatchItemSourceViewModel(
                    match,
                    baseFixture.NavigationService, baseFixture.DepdendencyResolver,
                    baseFixture.EventAggregator,
                    hubConnection));

            viewModel = new ScoresViewModel(
                baseFixture.NavigationService,
                baseFixture.DepdendencyResolver,
                baseFixture.EventAggregator,
                baseFixture.HubConnectionBuilder);
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
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
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
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnAppearing_PublishDateBarItemSelectedEvent_LoadDataByDateRange()
        {
            // Arrange
            matchService.GetMatches(
                  viewModel.SettingsService.UserSettings,
                  Arg.Is<DateRange>(dr => dr.FromDate == DateTime.Today.AddDays(-1) && dr.ToDate == DateTime.Today.EndOfDay()),
                  false).Returns(matchData);
            viewModel.OnAppearing();

            // Act
            viewModel.EventAggregator.GetEvent<DateBarItemSelectedEvent>().Publish(DateRange.FromYesterdayUntilNow());

            // Assert
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).Select(vm => vm.Match).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public void OnAppearing_Always_CallMatchServiceToSubscribeChanged()
        {
            // Act
            viewModel.OnAppearing();

            // Assert
            matchService.Received(1)
                .SubscribeMatches(hubConnection, Arg.Any<Action<string, Dictionary<string, MatchPayload>>>());
        }

        [Fact]
        public async Task OnAppearing_OnException_WriteLog()
        {
            // Arrange
            hubConnection
                .StartWithKeepAlive(TimeSpan.FromSeconds(30), Arg.Any<CancellationToken>())
                .ThrowsForAnyArgs(new Exception("exception"));

            // Act
            viewModel.OnAppearing();

            // Assert
            await viewModel.LoggingService.Received(1).LogErrorAsync(Arg.Is<Exception>(ex => ex.Message == "exception"));
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
            matchService.DidNotReceive().GetMatches(Arg.Any<UserSettings>(), Arg.Any<DateRange>(), Arg.Any<bool>());
        }

        [Fact]
        public void OnResume_Always_CallMatchServiceToSubscribeChanged()
        {
            // Act
            viewModel.OnResume();

            // Assert
            matchService.Received(1)
                .SubscribeMatches(hubConnection, Arg.Any<Action<string, Dictionary<string, MatchPayload>>>());
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
            const string sportId = "1";
            InitViewModelData(
                 out IMatchResult matchResult,
                 out IEnumerable<ITimeLine> matchTimelines,
                 out Dictionary<string, MatchPayload> matchPayloads);

            // Act
            viewModel.OnMatchesChanged(sportId, matchPayloads);

            // Assert
            var expectedMatch = viewModel.MatchItemSource
                .SelectMany(g => g)
                .FirstOrDefault(m => m.Match.Id == matchPayloads.FirstOrDefault().Key)?.Match;

            Assert.Equal(expectedMatch.MatchResult, matchResult);
            Assert.Equal(expectedMatch.TimeLines, matchTimelines);
        }

        [Fact]
        public void OnMatchChanged_SportIdIsNotCurrent_NotChangeMatchData()
        {
            // Arrange
            const string sportId = "2";
            InitViewModelData(
                out IMatchResult matchResult,
                out IEnumerable<ITimeLine> matchTimelines,
                out Dictionary<string, MatchPayload> matchPayloads);

            // Act
            viewModel.OnMatchesChanged(sportId, matchPayloads);

            // Assert
            var expectedMatch = viewModel.MatchItemSource
                .SelectMany(g => g)
                .FirstOrDefault(m => m.Match.Id == matchPayloads.FirstOrDefault().Key)?.Match;

            Assert.NotEqual(expectedMatch.MatchResult, matchResult);
            Assert.NotEqual(expectedMatch.TimeLines, matchTimelines);
        }

        private void InitViewModelData(out IMatchResult matchResult, out IEnumerable<ITimeLine> matchTimelines, out Dictionary<string, MatchPayload> matchPayloads)
        {
            matchResult = specimens.Create<IMatchResult>();
            matchTimelines = specimens.CreateMany<ITimeLine>();
            matchPayloads = new Dictionary<string, MatchPayload>
            {
                { matchData[0].Id, new MatchPayload {
                    MatchResult = matchResult,
                    Timelines = matchTimelines
                } },
            };
            matchService.GetMatches(viewModel.SettingsService.UserSettings, Arg.Any<DateRange>(), false).Returns(matchData);
            viewModel.OnNavigatingTo(null);
        }
    }
}