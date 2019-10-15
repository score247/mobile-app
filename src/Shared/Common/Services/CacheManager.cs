using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Caching;
using LiveScore.Common.Helpers;
using MethodTimer;

namespace LiveScore.Common.Services
{
    public interface ICacheManager
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool getLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool getLatestData = false);

        Task InvalidateAll();
    }

    public class CacheManager : ICacheManager
    {
        private readonly ICacheService cacheService;
        private readonly IList<string> cachedKeys;
        private readonly INetworkConnection networkConnectionManager;
        private readonly ILoggingService loggingService;

        public CacheManager(
            ICacheService cacheService,
            INetworkConnection networkConnectionManager,
            ILoggingService loggingService)
        {
            this.cacheService = cacheService;
            this.loggingService = loggingService;
            this.networkConnectionManager = networkConnectionManager;
            cachedKeys = new List<string>();
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                try
                {
                    if (getLatestData)
                    {
                        await GetFromApiAndSetToCacheAsync(key, factory, options);
                    }

                    var dataFromCache = await cacheService.GetAsync<T>(key).ConfigureAwait(false);

                    if (Equals(dataFromCache, default(T)))
                    {
                        await GetFromApiAndSetToCacheAsync(key, factory, options);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    // TODO: Temporary comment code here to find another solution
                    // networkConnectionManager.PublishConnectionTimeoutEvent();

                    await loggingService.LogExceptionAsync(ex);
                    return await cacheService.GetAsync<T>(key);
                }
            }
            else
            {
                networkConnectionManager.PublishNetworkConnectionEvent();
            }

            return await cacheService.GetAsync<T>(key).ConfigureAwait(false);
        }

        private async Task GetFromApiAndSetToCacheAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options)
        {
            Profiler.Start($"getLatestData method:{factory.Method.Name} key:{key}");

            var data = await factory.Invoke().ConfigureAwait(false);

            await SetCacheAsync(key, data, options).ConfigureAwait(false);
            Profiler.Stop($"getLatestData method:{factory.Method.Name} key:{key}");
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool getLatestData = false)
            => GetOrSetAsync(key, factory,
                new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(absoluteExpiredTime)),
                    getLatestData);

        public async Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool getLatestData = false)
            => await cacheService.GetOrSetAsync(key, factory, options).ConfigureAwait(false);

        public async Task InvalidateAll()
        {
            foreach (var key in cachedKeys)
            {
                await cacheService.RemoveAsync(key).ConfigureAwait(false);
            }

            cachedKeys.Clear();
        }

        [Time]
        private Task SetCacheAsync<T>(string key, T data, CacheItemOptions options)
        {
            if (Equals(data, default(T)))
            {
                return Task.CompletedTask;
            }

            if (!cachedKeys.Contains(key))
            {
                cachedKeys.Add(key);
            }

            return cacheService.SetAsync(key, data, options);
        }
    }
}