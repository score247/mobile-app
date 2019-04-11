﻿namespace LiveScore.Core.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Polly;
    using Polly.Wrap;
    using Refit;

    public interface IApiPolicy
    {
        Func<int, TimeSpan> SleepDurationProvider { get; }

        Task<T> WaitAndRetry<T>(Func<Task<T>> func);

        Task<T> Timeout<T>(Func<Task<T>> func);

        Task<T> RetryAndTimeout<T>(Func<Task<T>> func);
    }

    public class ApiPolicy : IApiPolicy
    {
        const int DEFAULT_COUNT = 3;
        const int TIMEOUT_SECONDS = 2;

        // Handle both exceptions and return values in one policy
        readonly HttpStatusCode[] httpStatusCodesWorthRetrying =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public Task<T> WaitAndRetry<T>(Func<Task<T>> func)
        => Policy
            .Handle<WebException>()
            .Or<ApiException>(ex => httpStatusCodesWorthRetrying.Contains(ex.StatusCode))
            .WaitAndRetryAsync(DEFAULT_COUNT, SleepDurationProvider)
            .ExecuteAsync<T>(func);

        public Task<T> Timeout<T>(Func<Task<T>> func)
            => Policy.TimeoutAsync(TIMEOUT_SECONDS).ExecuteAsync<T>(func);

        public Task<T> RetryAndTimeout<T>(Func<Task<T>> func)
        {
            var retryPolicy = Policy
            .Handle<WebException>()
            .Or<ApiException>(ex => httpStatusCodesWorthRetrying.Contains(ex.StatusCode))           
            .Retry(DEFAULT_COUNT, onRetry: (exception, retryCount, context) =>
            {
                //TODO LOG EXCEPTION
                //logger.Error($"Retry {retryCount} of {context.PolicyKey} at {context.ExecutionKey}, getting {context["Type"]} of id {context["Id"]}, due to: {exception}.");
            });

            var timeoutPolicy = Policy.Timeout(TIMEOUT_SECONDS);

            PolicyWrap commonResilience = Policy.Wrap(retryPolicy, timeoutPolicy);

            return commonResilience.Execute(func);
        }

        public Func<int, TimeSpan> SleepDurationProvider => retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
    }
}