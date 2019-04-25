using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Settings;
using LiveScore.Core.Services;
using LiveScore.Soccer.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Soccer.Tests.Services
{
    public class MatchServiceTests
    {
        private readonly IApiService mockApiService;
        private readonly ILocalStorage mockCache;
        private readonly ILoggingService mockLogger;
        private readonly IApiPolicy mockPolicy;

        private readonly MatchService matchService;

        public MatchServiceTests()
        {   
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
            await mockCache.Received(1).GetAndFetchLatestValue(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Match>>>>(), false, Arg.Any<DateTime>());
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
        public async Task GetMatches_HasValue_ShouldReturnCorrectListCount()
        {
            // Arrange
            var settings = new UserSettings("1", "en", "+07:00");
            var dateRange = new DateRange();
            var matchList = new List<Match>
            {
                new Match{ Id = "match:1", EventDate = DateTime.Now, MatchResult = new MatchResult{ AwayScore = 1, HomeScore = 2 } },
                new Match{ Id = "match:2", EventDate = DateTime.Now, MatchResult = new MatchResult{ AwayScore = 1, HomeScore = 2 } }
            };

            mockCache.GetAndFetchLatestValue(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Match>>>>(), false, Arg.Any<DateTime>())
                .Returns(matchList);

            // Act
            var matches = await matchService.GetMatches(settings, dateRange);

            // Assert
            Assert.Equal(matchList.Count, matches.Count);
        }
    }
}
