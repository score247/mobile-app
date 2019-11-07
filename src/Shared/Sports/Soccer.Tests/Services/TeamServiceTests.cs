using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common.Services;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Soccer.Tests.Services
{
    public class TeamServiceTests : IClassFixture<CommonFixture>
    {
        private const string HomeTeamId = "sr:home";
        private const string AwayTeamId = "sr:away";

        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService mockApiService;
        private readonly ILoggingService mockLogger;
        private readonly ITeamService teamService;

        public TeamServiceTests(CommonFixture commonFixture)
        {
            fixture = commonFixture.Specimens;
            comparer = commonFixture.Comparer;
            mockLogger = Substitute.For<ILoggingService>();
            mockApiService = Substitute.For<IApiService>();

            teamService = new TeamService(mockApiService, mockLogger);
        }

        [Fact]
        public async Task GetHeadToHeadsAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange            
            mockApiService
                .Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("Api Exception"));

            // Act
            var headToHeads = await teamService.GetHeadToHeadsAsync(HomeTeamId, AwayTeamId, "en_US");

            // Assert
            await mockLogger.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());

            Assert.Null(headToHeads);
        }

        [Fact]
        public async Task GetHeadToHeadsAsync_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var expectedH2H = fixture.Create<IEnumerable<SoccerMatch>>();

            mockApiService
                .Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .Returns(expectedH2H);

            // Act
            var headToHeads = await teamService.GetHeadToHeadsAsync(HomeTeamId, AwayTeamId, "en_US");

            // Assert
            Assert.True(comparer.Compare(expectedH2H, headToHeads).AreEqual);
        }

        [Fact]
        public async Task GetTeamResultsAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange            
            mockApiService
                .Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("Api Exception"));

            // Act
            var teamResults = await teamService.GetTeamResultsAsync(HomeTeamId, AwayTeamId, "en_US");

            // Assert
            await mockLogger.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());

            Assert.Null(teamResults);
        }

        [Fact]
        public async Task GetTeamResultsAsync_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var expectedResults = fixture.Create<IEnumerable<SoccerMatch>>();

            mockApiService
                .Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .Returns(expectedResults);

            // Act
            var teamResults = await teamService.GetTeamResultsAsync(HomeTeamId, AwayTeamId, "en_US");

            // Assert
            Assert.True(comparer.Compare(expectedResults, teamResults).AreEqual);
        }
    }
}
