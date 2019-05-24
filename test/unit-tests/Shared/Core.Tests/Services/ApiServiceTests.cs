namespace LiveScore.Core.Tests.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using LiveScore.Core.Services;
    using NSubstitute;
    using Refit;
    using Xunit;

    public interface IMockApi
    {
        [Get("/Match/GetMatcheIds?sportId={sportId}")]
        Task<string> GetMatcheIds(string sportId);
    }

    public class MockModel { }

    public class ApiServiceTests
    {
        [Fact]
        public void GetApi_Always_GetFromRefitRestService()
        {
            // Arrange
            var apiPolicy = Substitute.For<IApiPolicy>();
            var apiService = new ApiService(apiPolicy);
            var mockApi = apiService.GetApi<IMockApi>();

            // Act
            var httpClient = (HttpClient)mockApi.GetType().GetProperty("Client").GetValue(mockApi);

            // Assert
            Assert.Equal("https://testing2.nexdev.net/V1/api", httpClient.BaseAddress.ToString());
        }

        [Fact]
        public void Execute_Always_CallApiPolicyRetryAndTimeout()
        {
            // Arrange
            var apiPolicy = Substitute.For<IApiPolicy>();
            var apiService = new ApiService(apiPolicy);
            Task<MockModel> func() => Task.FromResult(new MockModel());

            // Act
            apiService.Execute(func);

            // Assert
            apiPolicy.Received(1).RetryAndTimeout(func);
        }
    }
}