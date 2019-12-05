using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Soccer.Tests.Services
{
    public class SoccerMatchServiceTests : IClassFixture<CommonFixture>
    {
        private readonly CompareLogic comparer;
        private readonly Fixture fixture;
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly ILoggingService loggingService;
        private readonly ISoccerMatchService matchService;

        public SoccerMatchServiceTests(CommonFixture commonFixture)
        {
            comparer = commonFixture.Comparer;
            fixture = commonFixture.Specimens;
            apiService = Substitute.For<IApiService>();
            cacheManager = Substitute.For<ICacheManager>();
            loggingService = Substitute.For<ILoggingService>();


            matchService = new MatchService(apiService, cacheManager, loggingService);
        }

        [Fact]
        public async Task GetMatchAsync_ThrowsException_InjectLoggingServiceAndReturnObjectNotNull()
        {
            // Arrange           
            apiService.Execute(Arg.Any<Func<Task<MatchInfo>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var match = await matchService.GetMatchAsync("sr:match", Language.English, DateTime.Now);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchAsync_HasValue_ShouldReturnCorrectDataFromApi()
        {
            // Arrange
            var matchId = fixture.Create<string>();
            var expectedMatch = fixture.Create<MatchInfo>();

            apiService.Execute(Arg.Any<Func<Task<MatchInfo>>>()).Returns(expectedMatch);

            // Act
            var match = await matchService.GetMatchAsync(matchId, Language.English, DateTime.Now);

            // Assert
            Assert.True(comparer.Compare(expectedMatch, match).AreEqual);
        }

        [Fact]
        public async Task GetMatchCoverageAsync_ThrowsException_InjectLoggingServiceAndReturnObjectNotNull()
        {
            // Arrange     
            cacheManager
                .GetOrSetAsync(Arg.Any<string>(), Arg.Any<Func<Task<MatchCoverage>>>(), Arg.Any<int>(), true)
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var match = await matchService.GetMatchCoverageAsync("sr:match", Language.English, DateTime.Now, true);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchCoverageAsync_HasValue_ShouldReturnCorrectDataFromApi()
        {
            // Arrange
            var matchId = fixture.Create<string>();
            var expectedMatch = fixture.Create<MatchCoverage>();

            cacheManager
                .GetOrSetAsync(Arg.Any<string>(), Arg.Any<Func<Task<MatchCoverage>>>(), Arg.Any<int>(), true)
                .Returns(expectedMatch);

            // Act
            var match = await matchService.GetMatchCoverageAsync(matchId, Language.English, DateTime.Now, true);

            // Assert
            Assert.True(comparer.Compare(expectedMatch, match).AreEqual);
        }

        [Fact]
        public async Task GetMatchCommentaries_ThrowsException_InjectLoggingServiceAndReturnObjectNotNull()
        {
            // Arrange           
            apiService.Execute(Arg.Any<Func<Task<IEnumerable<MatchCommentary>>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var match = await matchService.GetMatchCommentariesAsync("sr:match", Language.English, DateTime.Now);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchCommentariesAsync_HasValue_ShouldReturnCorrectDataFromApi()
        {
            // Arrange
            var matchId = fixture.Create<string>();
            var expected = fixture.CreateMany<MatchCommentary>();

            apiService.Execute(Arg.Any<Func<Task<IEnumerable<MatchCommentary>>>>()).Returns(expected);

            // Act
            var actual = await matchService.GetMatchCommentariesAsync(matchId, Language.English, DateTime.Now);

            // Assert
            Assert.True(comparer.Compare(expected, actual).AreEqual);
        }

        [Fact]
        public async Task GetMatchStatisticAsync_ThrowsException_InjectLoggingServiceAndReturnObjectNotNull()
        {
            // Arrange           
            apiService.Execute(Arg.Any<Func<Task<MatchStatistic>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var match = await matchService.GetMatchStatisticAsync("sr:match", Language.English, DateTime.Now);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchStatisticAsync_HasValue_ShouldReturnCorrectDataFromApi()
        {
            // Arrange
            var matchId = fixture.Create<string>();
            var expected = fixture.Create<MatchStatistic>();

            apiService.Execute(Arg.Any<Func<Task<MatchStatistic>>>()).Returns(expected);

            // Act
            var actual = await matchService.GetMatchStatisticAsync(matchId, Language.English, DateTime.Now);

            // Assert
            Assert.True(comparer.Compare(expected, actual).AreEqual);
        }

        [Fact]
        public async Task GetMatchLineupsAsync_ThrowsException_InjectLoggingServiceAndReturnObjectNotNull()
        {
            // Arrange           
            apiService.Execute(Arg.Any<Func<Task<MatchLineups>>>())
                .ThrowsForAnyArgs(new InvalidOperationException("NotFound"));

            // Act
            var match = await matchService.GetMatchLineupsAsync("sr:match", Language.English, DateTime.Now);

            // Assert
            await loggingService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.NotNull(match);
        }

        [Fact]
        public async Task GetMatchLineupsAsync_HasValue_ShouldReturnCorrectDataFromApi()
        {
            // Arrange
            var matchId = fixture.Create<string>();
            var expected = fixture.Create<MatchLineups>();

            apiService.Execute(Arg.Any<Func<Task<MatchLineups>>>()).Returns(expected);

            // Act
            var actual = await matchService.GetMatchLineupsAsync(matchId, Language.English, DateTime.Now);

            // Assert
            Assert.True(comparer.Compare(expected, actual).AreEqual);
        }
    }
}
