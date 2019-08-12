namespace Soccer.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using Xunit;

    public class OddsServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService mockApiService;
        private readonly ICachingService mockCache;
        private readonly ILoggingService mockLogger;
        private readonly IOddsService oddsService;

        public OddsServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Specimens;
            mockCache = Substitute.For<ICachingService>();
            mockLogger = Substitute.For<ILoggingService>();
            mockApiService = Substitute.For<IApiService>();

            oddsService = new OddsService(mockCache, mockLogger, mockApiService);
        }

        [Fact]
        public async Task GetOdds_WhenCall_InjectCacheService()
        {
            // Arrange

            // Act
            await oddsService.GetOdds("en", "sr:match:1", 1, "decimal");

            // Assert
            await mockCache.Received(1)
                .GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<MatchOdds>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    null);
        }

        [Fact]
        public async Task GetOdds_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            mockCache.GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<MatchOdds>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    Arg.Any<DateTimeOffset>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var odds = await oddsService.GetOdds("en", "sr:match:1", 1, "decimal");

            // Assert
            mockLogger.Received(1).LogError(Arg.Any<InvalidOperationException>());
            Assert.NotNull(odds);
        }

        [Fact]
        public async Task GetOdds_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var expectedMatchOdds = fixture.Create<MatchOdds>();

            mockCache.GetAndFetchLatestValue(
                "Odds-sr:match:1-1-decimal",
                Arg.Any<Func<Task<MatchOdds>>>(),
                Arg.Any<Func<DateTimeOffset, bool>>(),
                null).Returns(expectedMatchOdds);

            // Act
            var actualOdds = await oddsService.GetOdds("en", "sr:match:1", 1, "decimal");

            // Assert
            Assert.True(comparer.Compare(expectedMatchOdds, actualOdds).AreEqual);
        }

        [Fact]
        public async Task GetOddsMovement_WhenCall_InjectCacheService()
        {
            // Arrange

            // Act
            await oddsService.GetOddsMovement("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            await mockCache.Received(1)
                .GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<MatchOddsMovement>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    null);
        }

        [Fact]
        public async Task GetOddsMovement_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            mockCache.GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<MatchOddsMovement>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    Arg.Any<DateTimeOffset>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var odds = await oddsService.GetOddsMovement("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            mockLogger.Received(1).LogError(Arg.Any<InvalidOperationException>());
            Assert.NotNull(odds);
        }

        [Fact]
        public async Task GetOddsMovement_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var expectedMatchOdds = fixture.Create<MatchOddsMovement>();

            mockCache.GetAndFetchLatestValue(
                "OddsMovement-sr:match:1-1-dec-sr:book:1",
                Arg.Any<Func<Task<MatchOddsMovement>>>(),
                Arg.Any<Func<DateTimeOffset, bool>>(),
                null).Returns(expectedMatchOdds);

            // Act
            var actualOdds = await oddsService.GetOddsMovement("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            Assert.True(comparer.Compare(expectedMatchOdds, actualOdds).AreEqual);
        }
    }
}