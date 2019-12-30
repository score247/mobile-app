using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Soccer.Tests.Services
{
    public class MatchServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService apiService;
        private readonly ILoggingService loggingService;
        private readonly IMatchService matchService;

        public MatchServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Specimens;
            apiService = Substitute.For<IApiService>();
            loggingService = Substitute.For<ILoggingService>();

            matchService = new MatchService(apiService, loggingService);
        }

        [Fact]
        public async Task GetMatches_WhenCall_ApiServiceExecute()
        {
            // Arrange
            var dateTime = DateTime.Now;

            // Act
            await matchService.GetMatchesByDateAsync(dateTime, Language.English);

            // Assert
            await apiService.Received(1)
                .Execute(
                    Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>());
        }

        [Fact]
        public async Task GetMatches_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            var dateTime = DateTime.Now;
            apiService.Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var matches = await matchService.GetMatchesByDateAsync(dateTime, Language.English);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.Empty(matches);
        }

        [Fact]
        public async Task GetMatches_HasValue_ShouldReturnCorrectListCountFromApi()
        {
            // Arrange
            var dateTime = DateTime.Now;

            var expectedMatches = fixture.CreateMany<SoccerMatch>();

            apiService.Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>()).Returns(expectedMatches);

            // Act
            var actualMatches = await matchService.GetMatchesByDateAsync(dateTime, Language.English);

            // Assert
            Assert.True(comparer.Compare(expectedMatches, actualMatches).AreEqual);
        }

        [Fact]
        public async Task GetLiveMatchesAsync_CacheHasValue_ReturnDataFromCache()
        {
            // Arrange
            var expectedMatches = fixture.CreateMany<SoccerMatch>();

            apiService.Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>()).Returns(expectedMatches);

            // Act
            var actualMatches = await matchService.GetLiveMatchesAsync(Language.English);

            // Assert
            Assert.Equal(expectedMatches, actualMatches);
        }

        [Fact]
        public async Task GetLiveMatchesAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            apiService.Execute(Arg.Any<Func<Task<IEnumerable<SoccerMatch>>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var matches = await matchService.GetLiveMatchesAsync(Language.English);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.Empty(matches);
        }
    }
}