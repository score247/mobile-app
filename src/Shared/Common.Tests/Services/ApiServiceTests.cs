namespace LiveScore.Core.Tests.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LiveScore.Common;
    using LiveScore.Common.Helpers;
    using LiveScore.Common.Services;
    using NSubstitute;
    using Refit;
    using Xunit;

    public interface IMockApi
    {
        [Get("/Match/GetMatchIds?sportId={sportId}")]
        Task<string> GetMatchIds(string sportId);
    }

    public class MockModel { }

    public class ApiServiceTests
    {
        [Fact]
        public void GetApi_Always_GetFromRefitRestService()
        {
            // Arrange
            var httpService = Substitute.For<IHttpClientFactory>();
            httpService.CreateClient(nameof(ApiService)).Returns(new HttpClient()
            {
                BaseAddress = new Uri("https://score247-api1.nexdev.net/dev/api")
            });

            var apiService = new ApiService(
                httpService,
                new RefitSettings(),
                Substitute.For<ICacheManager>(),
                Substitute.For<ILoggingService>(),
                Substitute.For<IDeviceInfo>(),
                Substitute.For<ICryptographyHelper>(),
                Substitute.For<IConfiguration>());
            var mockApi = apiService.GetApi<IMockApi>();

            // Act
            var httpClient = (HttpClient)mockApi.GetType().GetProperty("Client").GetValue(mockApi);

            // Assert
            Assert.Equal("https://score247-api1.nexdev.net/dev/api", httpClient.BaseAddress.ToString());
        }
    }
}