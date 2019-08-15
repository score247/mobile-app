namespace Soccer.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using Xunit;

    public class MatchServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService apiService;
        private readonly ICachingService cacheService;
        private readonly ILoggingService loggingService;
        private readonly IMatchService matchService;

        public MatchServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Specimens;
            apiService = Substitute.For<IApiService>();
            cacheService = Substitute.For<ICachingService>();
            loggingService = Substitute.For<ILoggingService>();


            matchService = new MatchService(apiService, cacheService, loggingService);
        }

        [Fact]
        public async Task GetMatches_WhenCall_InjectCacheService()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();

            // Act
            await matchService.GetMatches(dateRange, Language.English);

            // Assert
            await cacheService.Received(1)
                .GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<IEnumerable<Match>>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    null);
        }

        [Fact]
        public async Task GetMatches_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();
            cacheService.GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<IEnumerable<Match>>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    Arg.Any<DateTimeOffset>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var matches = await matchService.GetMatches(dateRange, Language.English);

            // Assert
            loggingService.Received(1).LogError(Arg.Any<InvalidOperationException>());
            Assert.Empty(matches);
        }

        [Fact]
        public async Task GetMatches_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var dateTime = DateTime.Now;
            var dateRange = new DateRange(dateTime);
            var cacheKey = $"Matches:{dateTime.Date}:en-US";

            var expectedMatches = fixture.CreateMany<Match>();

            cacheService.GetAndFetchLatestValue(
                cacheKey,
                Arg.Any<Func<Task<IEnumerable<Match>>>>(),
                Arg.Any<Func<DateTimeOffset, bool>>(),
                null).Returns(expectedMatches);

            // Act
            var actualMatches = await matchService.GetMatches(dateRange, Language.English, true);

            // Assert
            Assert.True(comparer.Compare(expectedMatches, actualMatches).AreEqual);
        }

        [Fact]
        public async Task GetMatch_CacheHasValue_ReturnDataFromCache()
        {
            // Arrange
            var matchId = new Fixture().Create<string>();
            var cacheKey = $"Match:{matchId}:en-US";
            var expectedMatch = fixture.Create<Match>();

            cacheService.GetAndFetchLatestValue(
               cacheKey,
               Arg.Any<Func<Task<Match>>>(),
               Arg.Any<Func<DateTimeOffset, bool>>(),
               null).Returns(expectedMatch);

            // Act
            var actualMatch = await matchService.GetMatch(matchId, Language.English, false);

            // Assert
            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        public async Task GetMatch_Exception_WriteLog()
        {
            // Arrange
            var settings = new UserSettings("1", "en-US", "+07:00");

            cacheService.GetAndFetchLatestValue(
               "Match-1-en-US-+07:00-123",
               Arg.Any<Func<Task<Match>>>(),
               Arg.Any<Func<DateTimeOffset, bool>>(),
               null).ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var actualMatch = await matchService.GetMatch("123", Language.English, false);

            // Assert
            loggingService.Received(1).LogError(Arg.Any<InvalidOperationException>());
            Assert.Null(actualMatch);
        }
    }
}