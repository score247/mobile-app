namespace LiveScore.Core.Tests.Services
{
    using LiveScore.Core.Services;
    using Refit;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public interface IMockApi
    {
        [Get("/Match/GetMatcheIds?sportId={sportId}")]
        Task<string> GetMatcheIds(string sportId);
    }

    public class ApiServiceTests
    {
        [Fact]
        public void GetApi_Always_GetFromRefitRestService()
        {
            // Arrange
            var apiService = new ApiService();
            var mockApi = apiService.GetApi<IMockApi>();

            // Act
            var httpClient = (HttpClient)mockApi.GetType().GetProperty("Client").GetValue(mockApi);

            // Assert
            Assert.Equal("https://testing2.nexdev.net/Main/api", httpClient.BaseAddress.ToString());
        }
    }
}