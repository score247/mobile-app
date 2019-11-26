using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static LiveScore.Soccer.Services.SoccerApi;

namespace Soccer.Tests.Services
{
    public class LeagueServiceTests
    {
        private readonly Fixture specimens;
        private readonly IApiService subApiService;
        private readonly ICacheManager subCacheManager;
        private readonly ILoggingService subLoggingService;
        private readonly LeagueApi subLeagueApi;
        private readonly ILeagueService leagueService;

        public LeagueServiceTests()
        {
            specimens = new Fixture();

            subApiService = Substitute.For<IApiService>();
            subCacheManager = Substitute.For<ICacheManager>();
            subLoggingService = Substitute.For<ILoggingService>();
            subLeagueApi = Substitute.For<LeagueApi>();

            leagueService = new LeagueService(
               subApiService,
               subCacheManager,
               subLoggingService,
               subLeagueApi);
        }

        [Fact]
        public async Task GetTable_NoException_ReturnTableFromApiService()
        {
            // Arrange
            const string leagueId = "1";
            const string seasonId = "2";
            const string leagueRoundGroup = "A";
            var language = Language.English;
            var expectedTable = specimens.Create<LeagueTable>();
            subApiService.Execute(Arg.Any<Func<Task<LeagueTable>>>()).Returns(expectedTable);

            // Act
            var actualTable = await leagueService.GetTable(leagueId, seasonId, leagueRoundGroup, language);

            // Assert
            Assert.Equal(expectedTable, actualTable);
        }

        [Fact]
        public async Task GetTable_HasException_ReturnNullAndWriteLog()
        {
            // Arrange
            const string leagueId = "1";
            const string seasonId = "2";
            const string leagueRoundGroup = "A";
            var language = Language.English;
            var exception = await ApiException.Create(new HttpRequestMessage(), HttpMethod.Post, new HttpResponseMessage());
            subApiService.Execute(Arg.Any<Func<Task<LeagueTable>>>()).Throws(exception);

            // Act
            var actualTable = await leagueService.GetTable(leagueId, seasonId, leagueRoundGroup, language);

            // Assert
            await subLoggingService.Received().LogExceptionAsync(
                Arg.Is(exception),
                Arg.Is($"Response: {exception?.Content} \r\nRequest URL: {exception?.RequestMessage?.RequestUri}"));
            Assert.Null(actualTable);
        }
    }
}