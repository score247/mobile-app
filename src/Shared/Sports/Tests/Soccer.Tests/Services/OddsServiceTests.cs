namespace Soccer.Tests.Services
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
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
            var settings = new UserSettings("1", "en", "+07:00");
           

            // Act
            await oddsService.GetOdds(settings, "sr:match:1", 1);

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
            var settings = new UserSettings("1", "en", "+07:00");            
            mockCache.GetAndFetchLatestValue(
                    Arg.Any<string>(),
                    Arg.Any<Func<Task<MatchOdds>>>(),
                    Arg.Any<Func<DateTimeOffset, bool>>(),
                    Arg.Any<DateTimeOffset>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound Key"));

            // Act
            var odds = await oddsService.GetOdds(settings, "sr:match:1", 1);

            // Assert
            mockLogger.Received(1).LogError(Arg.Any<InvalidOperationException>());
            Assert.NotNull(odds);
        }
    }
}
