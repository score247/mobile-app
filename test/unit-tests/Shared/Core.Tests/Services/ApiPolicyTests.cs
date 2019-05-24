using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LiveScore.Core.Services;
using Refit;
using Xunit;

namespace LiveScore.Core.Tests.Services
{
    public class ApiPolicyTests
    {
        private readonly IApiPolicy apiPolicy;

        public ApiPolicyTests()
        {
            apiPolicy = new ApiPolicy();
        }

        [Fact]
        public async Task RetryAndTimeout_ExcuteWithoutError()
        {
            // Arrange            

            // Act            
            await apiPolicy.RetryAndTimeout(() => SumFunc(1, 2));

            // Assert

        }

        [Theory]
        [InlineData(HttpStatusCode.RequestTimeout)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        public async Task RetryAndTimeout_ExcuteFuncThatThrowsException(HttpStatusCode statusCode)
        {
            // Arrange            

            // Act            
            var exception = await Assert.ThrowsAsync<ApiException>
                (
                    async () => await apiPolicy.RetryAndTimeout(() => FuncThrowsException(statusCode))
                );

            // Assert
            Assert.IsType<ApiException>(exception);
        }

        private async Task<int> SumFunc(int a1, int a2)
        {
            await Task.Delay(1);

            return a1 + a2;
        }

        private async Task<int> FuncThrowsException(HttpStatusCode statusCode)
        {
            var exception = await ApiException.Create(new HttpRequestMessage(), new HttpMethod("get"), new HttpResponseMessage(statusCode));

            throw exception;
        }
    }
}
