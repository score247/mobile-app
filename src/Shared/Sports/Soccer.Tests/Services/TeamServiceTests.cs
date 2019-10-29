using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
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
        private readonly ICacheManager mockCache;
        private readonly ILoggingService mockLogger;
        private readonly ITeamService teamService;

        public TeamServiceTests(CommonFixture commonFixture)
        {
            fixture = commonFixture.Specimens;
            comparer = commonFixture.Comparer;
            mockCache = Substitute.For<ICacheManager>();
            mockLogger = Substitute.For<ILoggingService>();
            mockApiService = Substitute.For<IApiService>();

            teamService = new TeamService(mockApiService, mockCache, mockLogger);
        }

        [Fact]
        public async Task GetHeadToHeadsAsync_NotForceFetchNew_InjectCacheService()
        {
            // Arrange           

            // Act
            var headToHeads = await teamService.GetHeadToHeadsAsync(HomeTeamId, AwayTeamId, "en_US", false);

            // Assert
            await mockCache.Received(1)
                .GetOrSetAsync(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>(),
                    Arg.Any<int>(),
                    false);

            Assert.NotNull(headToHeads);
        }

        [Fact]
        public async Task GetHeadToHeadsAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange            
            mockCache.GetOrSetAsync(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>(),
                    Arg.Any<int>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

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

            mockCache
                .GetOrSetAsync(
                    $"LiveScore.Soccer.Services.TeamService.GetHeadToHeadsAsync:{HomeTeamId}:{AwayTeamId}:en_US",
                    Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>(),
                    Arg.Any<int>())
                .Returns(expectedH2H);

            // Act
            var headToHeads = await teamService.GetHeadToHeadsAsync(HomeTeamId, AwayTeamId, "en_US");

            // Assert
            Assert.True(comparer.Compare(expectedH2H, headToHeads).AreEqual);
        }
    }
}
