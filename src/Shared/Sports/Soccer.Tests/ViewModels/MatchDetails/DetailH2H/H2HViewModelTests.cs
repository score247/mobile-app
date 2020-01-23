using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common;
using LiveScore.Common.Services;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Prism.Events;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly SoccerMatch match;
        private readonly ITeamService teamService;
        private readonly ILoggingService logService;
        private readonly Fixture fixture;

        private readonly H2HViewModel viewModel;

        public H2HViewModelTests(ViewModelBaseFixture baseFixture)
        {
            fixture = baseFixture.CommonFixture.Specimens;
            match = fixture.Create<SoccerMatch>();

            teamService = Substitute.For<ITeamService>();
            var matchStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            logService = Substitute.For<ILoggingService>();
            var buildFlagUrlFunc = Substitute.For<Func<string, string>>();
            var eventAggregator = Substitute.For<IEventAggregator>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            baseFixture.DependencyResolver.Resolve<ITeamService>("1").Returns(teamService);
            baseFixture.DependencyResolver.Resolve<IMatchDisplayStatusBuilder>("1").Returns(matchStatusBuilder);
            baseFixture.DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName).Returns(buildFlagUrlFunc);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(logService);
            baseFixture.DependencyResolver.Resolve<INetworkConnection>().Returns(networkConnectionManager);

            viewModel = new H2HViewModel(
                match,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_Always_InjectGetHeadToHeadsAsync()
        {
            // Arrange

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            await teamService.Received(1).GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName);
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_Exception_ShouldLogError()
        {
            // Arrange
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Throws(new InvalidOperationException("Error"));

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            await logService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_Null_HasDataShouldBeFalse()
        {
            // Arrange
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(Task.FromResult(default(IEnumerable<IMatch>)));

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_NotStartMatches_ShouldHideStats()
        {
            // Arrange
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(new List<SoccerMatch>
                {
                    new SoccerMatch(new MatchResult(MatchStatus.NotStarted, MatchStatus.NotStarted)),
                    new SoccerMatch(new MatchResult(MatchStatus.NotStarted, MatchStatus.NotStarted))
                });

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.True(viewModel.HasData);
            Assert.False(viewModel.VisibleStats);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_ContainsClosedMatches_ShouldVisibleStats()
        {
            // Arrange
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(new List<SoccerMatch>
                {
                    fixture.Create<SoccerMatch>()
                        .With(match => match.Id, "sr:match:1")
                        .With(match => match.EventStatus, MatchStatus.NotStarted),
                    fixture.Create<SoccerMatch>()
                        .With(match => match.Id, "sr:match:2")
                        .With(match => match.EventStatus, MatchStatus.Closed)
                        .With(match => match.MatchStatus, MatchStatus.Ended)
                });

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.VisibleStats);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_HasMatches_ShouldExcludeCurrentMatch()
        {
            // Arrange
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(new List<SoccerMatch>
                {
                    fixture.Create<SoccerMatch>().With(match => match.Id, "sr:match:1").With(match => match.EventStatus, MatchStatus.NotStarted),
                    match
                });

            // Act
            await viewModel.LoadHeadToHeadAsync();

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.True(viewModel.HasData);
            Assert.False(viewModel.VisibleStats);
        }

        [Fact]
        public async Task RefreshAsync_Always_GetLatest()
        {
            // Arrange

            // Act
            await viewModel.RefreshAsync();

            // Assert
            await teamService.Received(1).GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName);
            Assert.False(viewModel.IsRefreshing);
        }

        [Fact]
        public async Task LoadTeamResult_Home_VisibleHomeTeamResults()
        {
            // Arrange

            // Act
            await viewModel.LoadTeamResultAsync("home");

            // Assert
            Assert.False(viewModel.VisibleHeadToHead);
            Assert.Null(viewModel.Stats);
        }

        [Fact]
        public async Task LoadTeamResult_Away_VisibleAwayTeamResults()
        {
            // Arrange

            // Act
            await viewModel.LoadTeamResultAsync("away");

            // Assert
            Assert.False(viewModel.VisibleHeadToHead);
            Assert.Null(viewModel.Stats);
        }

        [Fact]
        public async Task OnTeamResultTapped_ExecuteForAway_VisibleAwayTeamResults()
        {
            // Arrange

            // Act
            await viewModel.OnTeamResultTappedCommand.ExecuteAsync("away");

            // Assert
            Assert.False(viewModel.VisibleHeadToHead);
        }

        [Fact]
        public async Task OnTeamResultTapped_ExecuteForHome_VisibleHomeTeamResults()
        {
            // Arrange

            // Act
            await viewModel.OnTeamResultTappedCommand.ExecuteAsync("home");

            // Assert
            Assert.False(viewModel.VisibleHeadToHead);
        }

        [Fact]
        public async Task OnHeadToHeadTapped_ExecuteAsync_VisibleHeadToHead()
        {
            // Arrange

            // Act
            await viewModel.OnHeadToHeadTappedCommand.ExecuteAsync();

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
        }

        [Fact]
        public async Task RefreshCommand_ExecuteAsync_GetLatest()
        {
            // Arrange

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await teamService.Received(1).GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName);
            Assert.False(viewModel.IsRefreshing);
        }

        [Fact]
        public async Task RefreshCommand_SelectedHome_GetTeamResults()
        {
            // Arrange
            await viewModel.OnTeamResultTappedCommand.ExecuteAsync("home");

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await teamService.Received(2).GetTeamResultsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName);
            Assert.False(viewModel.IsRefreshing);
        }

        [Fact]
        public async Task LoadTeamResultAsync_Home_HasData()
        {
            // Arrange
            teamService.GetTeamResultsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(new List<SoccerMatch>
                {
                    fixture.Create<SoccerMatch>().With(match => match.Id, "sr:match:1").With(match => match.EventStatus, MatchStatus.Closed),
                    fixture.Create<SoccerMatch>().With(match => match.Id, "sr:match:2").With(match => match.EventStatus, MatchStatus.Closed)
                });

            // Act
            await viewModel.LoadTeamResultAsync("home");

            // Assert
            Assert.True(viewModel.HasData);
            Assert.Equal(2, viewModel.GroupedMatches.Count);
        }

        [Fact]
        public async Task LoadTeamResultAsync_Away_HasData()
        {
            // Arrange
            teamService.GetTeamResultsAsync(match.AwayTeamId, match.HomeTeamId, viewModel.CurrentLanguage.DisplayName)
                .Returns(new List<SoccerMatch>
                {
                    fixture.Create<SoccerMatch>().With(match => match.Id, "sr:match:1").With(match => match.EventStatus, MatchStatus.Closed),
                    fixture.Create<SoccerMatch>().With(match => match.Id, "sr:match:2").With(match => match.EventStatus, MatchStatus.Closed)
                });

            // Act
            await viewModel.LoadTeamResultAsync("away");

            // Assert
            Assert.True(viewModel.HasData);
            Assert.Equal(2, viewModel.GroupedMatches.Count);
        }
    }
}