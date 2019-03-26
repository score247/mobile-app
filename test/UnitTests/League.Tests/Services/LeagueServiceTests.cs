using System.Threading.Tasks;
using League.Models;
namespace League.Tests.Services
{
    using Common.Services;
    using League.Services;
    using NSubstitute;
    using Xunit;

    public class LeagueServiceTests
    {
        private readonly ILeagueApi mockLeagueApi;
        private readonly ISettingsService mockSettingsService;
        private ILeagueService service;

        public LeagueServiceTests()
        {
            mockLeagueApi = Substitute.For<ILeagueApi>();
            mockSettingsService = Substitute.For<ISettingsService>();
            service = new LeagueService(mockLeagueApi, mockSettingsService);
        }
    }
}
