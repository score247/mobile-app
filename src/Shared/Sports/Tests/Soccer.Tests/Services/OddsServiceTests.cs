﻿namespace Soccer.Tests.Services
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class OddsServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService mockApiService;
        private readonly ILocalStorage mockCache;
        private readonly ILoggingService mockLogger;
        private readonly IOddsService oddsService;

        public OddsServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Specimens;
            mockCache = Substitute.For<ILocalStorage>();
            mockLogger = Substitute.For<ILoggingService>();
            mockApiService = Substitute.For<IApiService>();

            oddsService = new OddsService(mockCache, mockLogger, mockApiService);
        }

        [Fact]
        public async Task GetOdds_WhenCall_InjectCacheService()
        {
            // Arrange

            // Act
            await oddsService.GetOdds("sr:match:1", 1);

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
            var odds = await oddsService.GetOdds("sr:match:1", 1);

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
                "Odds-sr:match:1",
                Arg.Any<Func<Task<MatchOdds>>>(),
                Arg.Any<Func<DateTimeOffset, bool>>(),
                null).Returns(expectedMatchOdds);

            // Act
            var actualOdds = await oddsService.GetOdds("sr:match:1", 1);

            // Assert
            Assert.True(comparer.Compare(expectedMatchOdds, actualOdds).AreEqual);
        }
    }
}
