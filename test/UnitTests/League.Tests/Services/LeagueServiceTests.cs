namespace League.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Services;
    using League.Models;
    using League.Services;
    using NSubstitute;
    using Xunit;

    public class LeagueServiceTests
    {
        private const string UngroupedCategoryId = "sr:category:393";
        private readonly ILeagueApi mockLeagueApi;
        private readonly ISettingsService mockSettingsService;
        private readonly ILeagueService service;

        public LeagueServiceTests()
        {
            mockLeagueApi = Substitute.For<ILeagueApi>();
            mockSettingsService = Substitute.For<ISettingsService>();
            service = new LeagueService(mockLeagueApi, mockSettingsService);
        }

        [Fact]
        public async Task GetLeaguesAsync_EmptyGroups_NotCallLeagueApi()
        {
            // Arrange

            // Act
            await service.GetLeaguesAsync();

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
            await service.GetLeaguesAsync();

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
                Leagues = new List<Common.Models.MatchInfo.League>
                {
                    CreateMockLeagueData("sr:category:1", "K League 1"),
                    CreateMockLeagueData("sr:category:2", "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeaguesAsync();

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
                Leagues = new List<Common.Models.MatchInfo.League>
                {
                    CreateMockLeagueData("sr:category:1", "K League 1"),
                    CreateMockLeagueData("sr:category:1", "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeaguesAsync();

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
                Leagues = new List<Common.Models.MatchInfo.League>
                {
                    CreateMockLeagueData(UngroupedCategoryId, "K League 1"),
                    CreateMockLeagueData(UngroupedCategoryId, "K League 2")
                }
            });

            // Act
            var leagues = await service.GetLeaguesAsync();

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
                Leagues = new List<Common.Models.MatchInfo.League>
                {
                    CreateMockLeagueData(UngroupedCategoryId, "K League 1"),
                    CreateMockLeagueData(UngroupedCategoryId, "K League 2"),
                    CreateMockLeagueData("sr:category:1", "V League 1"),
                    CreateMockLeagueData("sr:category:1", "V League 2")
                }
            });

            // Act
            var leagues = await service.GetLeaguesAsync();

            // Assert
            Assert.Equal(3, leagues.Count);
        }

        private static Common.Models.MatchInfo.League CreateMockLeagueData(string categoryId, string leagueName)
        => new Common.Models.MatchInfo.League
        {
            Name = leagueName,
            Category = new Common.Models.MatchInfo.Category
            {
                Id = categoryId
            }
        };
    }
}
