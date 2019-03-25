using System.Threading.Tasks;
using League.Models;
namespace League.Tests.Services
{
    using League.Services;
    using NSubstitute;
    using Xunit;

    public class LeagueServiceTests
    {
        private readonly ILeagueApi mockLeagueApi;
        private ILeagueService service;

        public LeagueServiceTests()
        {
            mockLeagueApi = Substitute.For<ILeagueApi>();

            service = new LeagueService(mockLeagueApi);
        }
    }
}
