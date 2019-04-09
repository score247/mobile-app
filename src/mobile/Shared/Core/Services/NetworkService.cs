namespace LiveScore.Core.Services
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Polly;
    using Refit;

    public interface INetworkService
    {
        Task<T> WaitAndRetry<T>(Func<Task<T>> func,
                                Func<int, TimeSpan> sleepDurationProvider);

        Task<T> Timeout<T>(Func<Task<T>> func);
    }

    public class NetworkService : INetworkService
    {
        const int DEFAULT_COUNT = 3;
        const int TIMEOUT_SECONDS = 2;

        public Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider)
        => Policy
            .Handle<WebException>()
            .Or<ApiException>(ex => !ex.Message.ToLower().Contains("404"))
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(DEFAULT_COUNT, sleepDurationProvider)
            .ExecuteAsync<T>(func);

        public Task<T> Timeout<T>(Func<Task<T>> func)
            => Policy.TimeoutAsync(TIMEOUT_SECONDS).ExecuteAsync<T>(func);
    }
}
