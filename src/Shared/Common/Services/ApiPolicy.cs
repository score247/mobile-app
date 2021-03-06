using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Refit;

namespace LiveScore.Common.Services
{
    public interface IApiPolicy
    {
        Func<int, TimeSpan> SleepDurationProvider { get; }

        Task<T> RetryAndTimeout<T>(Func<Task<T>> func);
    }

    public class ApiPolicy : IApiPolicy
    {
        private const int DefaultCount = 2;
        private const int TimeoutSeconds = 10;
        private const int DefaultPow = 2;

        // Handle both exceptions and return values in one policy
        private readonly HttpStatusCode[] httpStatusCodesWorthRetrying =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout, // 504
        };

        private AsyncRetryPolicy WaitAndRetryPolicy()
            => Policy
                .Handle<WebException>()
                .Or<ApiException>(HandleApiException)
                .WaitAndRetryAsync(DefaultCount, SleepDurationProvider);

        public Task<T> RetryAndTimeout<T>(Func<Task<T>> func)
        {
            var retryPolicy = WaitAndRetryPolicy();
            var timeoutPolicy = Policy.TimeoutAsync(TimeoutSeconds);
            var commonResilience = Policy.WrapAsync(retryPolicy, timeoutPolicy);

            return commonResilience.ExecuteAsync(func);
        }

        private bool HandleApiException(ApiException ex)
        {
            Debug.WriteLine($"RetryAndTimeout {ex.StatusCode}");

            return httpStatusCodesWorthRetrying.Contains(ex.StatusCode);
        }

        public Func<int, TimeSpan> SleepDurationProvider
            => retryAttempt => TimeSpan.FromSeconds(Math.Pow(DefaultPow, retryAttempt));
    }
}