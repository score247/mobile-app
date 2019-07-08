namespace Soccer.Tests.Services
{
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Services;
    using NSubstitute;
    using Xunit;

    public class LeagueServiceTests
    {
        private readonly ISoccerLeagueApi mockSoccerApi;
        private readonly ILoggingService mockLogger;
        private readonly IApiPolicy mockPolicy;
        private readonly ILeagueService leagueService;

        public LeagueServiceTests()
        {
            mockSoccerApi = Substitute.For<ISoccerLeagueApi>();
            mockLogger = Substitute.For<ILoggingService>();
            mockPolicy = Substitute.For<IApiPolicy>();

            leagueService = new LeagueService(mockSoccerApi, mockLogger, mockPolicy);
        }

        [Fact]
        public async Task GetLeagues_WhenCall_ReturnEmptyList()
        {
            // Act
            var actual = await leagueService.GetLeagues();

            // Assert
            Assert.Empty(actual);
        }
    }
}