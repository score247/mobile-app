namespace LiveScore.League.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Services;
    using Core.Models.LeagueInfo;
    using Core.Services;
    using NSubstitute;
    using Soccer.Services;
    using Xunit;

    public class LeagueServiceTests
    {
        private const string UngroupedCategoryId = "sr:category:393";
        private readonly ILeagueApi mockLeagueApi;
        private readonly ILoggingService mockLogService;
        private readonly ISettingsService mockSettingsService;
        private readonly ILeagueService service;

        public LeagueServiceTests()
        {
            mockLeagueApi = Substitute.For<ILeagueApi>();
            mockSettingsService = Substitute.For<ISettingsService>();
            mockLogService = Substitute.For<ILoggingService>();
            service = new LeagueService(mockLeagueApi, mockSettingsService, mockLogService);
        }

        [Fact]
        public async Task GetLeaguesAsync_EmptyGroups_NotCallLeagueApi()
        {
            // Arrange

            // Act
            await service.GetLeagues();

            // Assert
            await mockLeagueApi.DidNotReceive().GetLeaguesByGroup(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetLeaguesAsync_HasGroups_CallLeagueApiBaseOnNumberOfGroups()
        {
            // Arrange
            var mockGroups = new[] { "eu", "as" };
            mockSettingsService.LeagueGroups.Returns(mockGroups);

            // Act
            await service.GetLeagues();

            // Assert
            await mockLeagueApi.Received(mockGroups.Length).GetLeaguesByGroup(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetLeaguesAsync_HasData_ReturnCorrectCount()
        {
            // Arrange
            var mockGroups = new[] { "eu" };
            mockSettingsService.LeagueGroups.Returns(mockGroups);
            mockLeagueApi.GetLeaguesByGroup(Arg.Any<string>(), "eu", Arg.Any<string>(), Arg.Any<string>()).Returns(new LeagueInfo
            {
                Leagues = new List<League>
                {
                    CreateMockLeagueData("sr:category:1", "K League 1"),
                    CreateMockLeagueData("sr:category:2", "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeagues();

            // Assert
            Assert.Equal(2, leagues.Count);
        }

        [Fact]
        public async Task GetLeaguesAsync_SameCategoryGroup_ReturnCorrectCount()
        {
            // Arrange
            var mockGroups = new[] { "eu" };
            mockSettingsService.LeagueGroups.Returns(mockGroups);
            mockLeagueApi.GetLeaguesByGroup(Arg.Any<string>(), "eu", Arg.Any<string>(), Arg.Any<string>()).Returns(new LeagueInfo
            {
                Leagues = new List<League>
                {
                    CreateMockLeagueData("sr:category:1", "K League 1"),
                    CreateMockLeagueData("sr:category:1", "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeagues();

            // Assert
            Assert.Single(leagues);
        }

        [Fact]
        public async Task GetLeaguesAsync_SameCategoryButUngrouped_ReturnCorrectCount()
        {
            // Arrange
            var mockGroups = new[] { "eu" };
            mockSettingsService.LeagueGroups.Returns(mockGroups);
            mockLeagueApi.GetLeaguesByGroup(Arg.Any<string>(), "eu", Arg.Any<string>(), Arg.Any<string>()).Returns(new LeagueInfo
            {
                Leagues = new List<League>
                {
                    CreateMockLeagueData(UngroupedCategoryId, "K League 1"),
                    CreateMockLeagueData(UngroupedCategoryId, "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeagues();

            // Assert
            Assert.Equal(2, leagues.Count);
        }

        [Fact]
        public async Task GetLeaguesAsync_SameCategoryAndUngrouped_ReturnCorrectCount()
        {
            // Arrange
            var mockGroups = new[] { "eu" };
            mockSettingsService.LeagueGroups.Returns(mockGroups);
            mockLeagueApi.GetLeaguesByGroup(Arg.Any<string>(), "eu", Arg.Any<string>(), Arg.Any<string>()).Returns(new LeagueInfo
            {
                Leagues = new List<League>
                {
                    CreateMockLeagueData(UngroupedCategoryId, "K League 1"),
                    CreateMockLeagueData(UngroupedCategoryId, "K League 2"),
                    CreateMockLeagueData("sr:category:1", "V League 1"),
                    CreateMockLeagueData("sr:category:1", "V League 2")
                }
            });

            // Act
            var leagues = await service.GetLeagues();

            // Assert
            Assert.Equal(3, leagues.Count);
        }

        private static League CreateMockLeagueData(string categoryId, string leagueName)
        => new League
        {
            Name = leagueName,
            Category = new Category
            {
                Id = categoryId
            }
        };
    }
}
