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
                                Func<int, TimeSpan> sleepDurationProvider,
                                int retryCount);
    }

    public class NetworkService : INetworkService
    {
        public Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount)
        => Policy
            .Handle<WebException>()
            .Or<ApiException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(retryCount, sleepDurationProvider)
            .ExecuteAsync<T>(func);

    }
}
