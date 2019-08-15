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
            var dateRange = new DateRange
            (
                new DateTime(2019, 04, 25),
                new DateTime(2019, 04, 25)
            );
            var expectedMatches = fixture.CreateMany<Match>();

            cacheService.GetAndFetchLatestValue(
                "GetMatches:2019-04-25T00:00:00+07:00-2019-04-25T23:59:59+07:00:en-US",
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
            var expectedMatch = fixture.Create<Match>();

            cacheService.GetAndFetchLatestValue(
               "GetMatch:123:en-US",
               Arg.Any<Func<Task<Match>>>(),
               Arg.Any<Func<DateTimeOffset, bool>>(),
               null).Returns(expectedMatch);

            // Act
            var actualMatch = await matchService.GetMatch("123", Language.English, false);

            // Assert
            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        public async Task GetMatch_Exception_WriteLog()
        {
            // Arrange
            cacheService.GetAndFetchLatestValue(
               "GetMatch:123:en-US",
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