namespace Soccer.Tests.Services
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class MatchServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService mockApiService;
        private readonly ILocalStorage mockCache;
        private readonly ILoggingService mockLogger;
        private readonly IApiPolicy mockPolicy;
        private readonly IMatchService matchService;

        public MatchServiceTests(CommonFixture commonFixture)
        {   
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Fixture;
            mockCache = Substitute.For<ILocalStorage>();
            mockLogger = Substitute.For<ILoggingService>();
            mockPolicy = Substitute.For<IApiPolicy>();
            mockApiService = Substitute.For<IApiService>();

            matchService = new MatchService(mockCache, mockLogger, mockPolicy, mockApiService);
        }

        [Fact]
        public async Task GetMatches_WhenCall_InjectCacheService()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();

            // Act
            await matchService.GetMatches(settings, dateRange);

            // Assert
            await mockCache.Received(1)
                .GetAndFetchLatestValue(
                    Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Match>>>>(), false, Arg.Any<DateTime>());
        }

        [Fact]
        public async Task GetMatches_ThrowsException_InjectLoggingService()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();
            mockCache.GetAndFetchLatestValue(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Match>>>>(), false, Arg.Any<DateTime>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            await matchService.GetMatches(settings, dateRange);

            // Assert
            mockLogger.Received(1).LogError(Arg.Any<InvalidOperationException>());
        }

        [Fact]
        public async Task GetMatches_ThrowsException_ShouldReturnEmptyList()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();
            mockCache.GetAndFetchLatestValue(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Match>>>>(), false, Arg.Any<DateTime>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var matches = await matchService.GetMatches(settings, dateRange);

            // Assert
            Assert.Empty(matches);
        }

        [Fact]
        public async Task GetMatches_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var settings = new UserSettings("1", "en-US", "+07:00");
            var dateRange = new DateRange
            {
                FromDate = new DateTime(2019, 04, 25),
                ToDate = new DateTime(2019, 04, 25)
            };
            var expectedMatches = fixture.CreateMany<Match>();

            mockCache.GetAndFetchLatestValue(
                "Scores-1-en-US-+07:00-2019-04-25T00:00:00+07:00-2019-04-25T00:00:00+07:00",
                Arg.Any<Func<Task<IEnumerable<Match>>>>(),
                false,
                Arg.Any<DateTime>()).Returns(expectedMatches);

            // Act
            var actualMatches = await matchService.GetMatches(settings, dateRange);

            // Assert
            Assert.True(comparer.Compare(expectedMatches, actualMatches).AreEqual);
        }
    }
}