namespace LiveScore.Core.Tests.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
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
            var httpService = Substitute.For<IHttpService>();
            httpService.HttpClient.Returns(new HttpClient()
            {
                BaseAddress = new Uri("https://score247-api1.nexdev.net/dev/api")
            });

            var apiService = new ApiService(apiPolicy, httpService, new RefitSettings());
            var mockApi = apiService.GetApi<IMockApi>();

            // Act
            var httpClient = (HttpClient)mockApi.GetType().GetProperty("Client").GetValue(mockApi);

            // Assert
            Assert.Equal("https://score247-api1.nexdev.net/dev/api", httpClient.BaseAddress.ToString());
        }

        [Fact]
        public void Execute_Always_CallApiPolicyRetryAndTimeout()
        {
            // Arrange
            var apiPolicy = Substitute.For<IApiPolicy>();
            var httpService = Substitute.For<IHttpService>();
            httpService.HttpClient.Returns(new HttpClient()
            {
                BaseAddress = new Uri("https://score247-api1.nexdev.net/dev/api")
            });
            var apiService = new ApiService(apiPolicy, httpService, new RefitSettings());
            Task<MockModel> func() => Task.FromResult(new MockModel());

            // Act
            apiService.Execute(func);

            // Assert
            apiPolicy.Received(1).RetryAndTimeout(func);
        }
    }
}