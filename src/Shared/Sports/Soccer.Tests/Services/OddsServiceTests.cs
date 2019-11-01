namespace Soccer.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using Xunit;

    public class OddsServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly IApiService mockApiService;
        private readonly ILoggingService mockLogger;
        private readonly IOddsService oddsService;

        public OddsServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            mockLogger = Substitute.For<ILoggingService>();
            mockApiService = Substitute.For<IApiService>();

            oddsService = new OddsService(mockLogger, mockApiService);
        }

        [Fact]
        public async Task GetOddsAsync_ForceFetchNew_GetAndFetchLatestValue()
        {
            // Arrange

            // Act
            var matchOdds = await oddsService.GetOddsAsync("en", "sr:match:1", 1, "decimal");

            // Assert
            await mockApiService.Received(1)
                .Execute(Arg.Any<Func<Task<MatchOdds>>>());

            Assert.Null(matchOdds);
        }

        [Fact]
        public async Task GetOddsAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            mockApiService
                .Execute(Arg.Any<Func<Task<MatchOdds>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("Api Exception"));

            // Act
            var odds = await oddsService.GetOddsAsync("en", "sr:match:1", 1, "decimal");

            // Assert
            await mockLogger.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());

            Assert.Null(odds);
        }

        [Fact]
        public async Task GetOddsAsync_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var expectedMatchOdds = StubOdds("sr:match:1", 1, "over under");

            mockApiService
                .Execute(Arg.Any<Func<Task<MatchOdds>>>())
                .Returns(expectedMatchOdds);

            // Act
            var actualOdds = await oddsService.GetOddsAsync("en", "sr:match:1", 1, "decimal");

            // Assert
            Assert.True(comparer.Compare(expectedMatchOdds, actualOdds).AreEqual);
        }

        [Fact]
        public async Task GetOddsMovementAsync_ForceFetchNew_GetAndFetchLatestValue()
        {
            // Arrange

            // Act
            await oddsService.GetOddsMovementAsync("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            await mockApiService
                .Received(1)
                .Execute(Arg.Any<Func<Task<MatchOddsMovement>>>());
        }

        [Fact]
        public async Task GetOddsMovementAsync_ThrowsException_InjectLoggingServiceAndReturnEmptyList()
        {
            // Arrange
            mockApiService
                .Execute(Arg.Any<Func<Task<MatchOddsMovement>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("Api Exception"));

            // Act
            var odds = await oddsService.GetOddsMovementAsync("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            await mockLogger.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.Null(odds);
        }

        [Fact]
        public async Task GetOddsMovementAsync_CacheHasValue_ShouldReturnCorrectListCountFromCache()
        {
            // Arrange
            var matchId = "sr:match:1";
            var expectedMatchOdds = StubOddsMovement(matchId);

            mockApiService
               .Execute(Arg.Any<Func<Task<MatchOddsMovement>>>())
               .Returns(expectedMatchOdds);

            // Act
            var actualOdds = await oddsService.GetOddsMovementAsync("en", "sr:match:1", 1, "dec", "sr:book:1");

            // Assert
            Assert.True(comparer.Compare(expectedMatchOdds, actualOdds).AreEqual);
        }

        private static MatchOdds StubOdds(string matchId, byte betTypeId, string betTypeName)
           => new MatchOdds(
               matchId,
               new List<BetTypeOdds>
               {
                    new BetTypeOdds(betTypeId, betTypeName, new Bookmaker("1", "William"), new List<BetOptionOdds>())
               }
               );

        private static MatchOddsMovement StubOddsMovement(string matchId)
           => new MatchOddsMovement(
               matchId,
               new Bookmaker("1", "William"),
               new List<OddsMovement>
               {
                    new OddsMovement("5'", 1, 0, true, new List<BetOptionOdds>(), DateTimeOffset.Now)
               }
               );
    }
}