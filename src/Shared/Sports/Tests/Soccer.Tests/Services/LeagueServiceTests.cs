using LiveScore.Common.Services;
using LiveScore.Core.Services;
using LiveScore.Soccer.Services;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Soccer.Tests.Services
{
    public class LeagueServiceTests
    {
        private readonly ISoccerLeagueApi mockSoccerApi;
        private readonly ILoggingService mockLogger;
        private readonly IApiPolicy mockPolicy;

        private readonly LeagueService leagueService;

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
            // Arrange           

            // Act
            var actual = await leagueService.GetLeagues();

            // Assert
            Assert.Empty(actual);
        }
    }    
}
