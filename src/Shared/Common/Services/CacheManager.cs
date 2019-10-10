namespace LiveScore.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Caching;

    public static class CacheDuration
    {
        public const int Short = 120; // 120 seconds
        public const int Long = 7200;
    }

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

        public CacheManager(
            ICacheService cacheService,
            INetworkConnection networkConnectionManager)
        {
            this.cacheService = cacheService;
            this.networkConnectionManager = networkConnectionManager;
            cachedKeys = new List<string>();
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                if (getLatestData)
                {
                    try
                    {
                        var data = await factory.Invoke().ConfigureAwait(false);

                        await SetCacheAsync(key, data, options).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        networkConnectionManager.PublishConnectionTimeoutEvent();
                        return await cacheService.GetAsync<T>(key);
                    }
                }

                var dataFromCache = await cacheService.GetAsync<T>(key).ConfigureAwait(false);

                if (Equals(dataFromCache, default(T)))
                {
                    try
                    {
                        var data = await factory.Invoke().ConfigureAwait(false);

                        await SetCacheAsync(key, data, options).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        networkConnectionManager.PublishConnectionTimeoutEvent();
                        return await cacheService.GetAsync<T>(key);
                    }
                }
            }
            else
            {
                networkConnectionManager.PublishNetworkConnectionEvent();
            }

            return await cacheService.GetAsync<T>(key).ConfigureAwait(false);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool getLatestData = false)
            => GetOrSetAsync(key, factory,
                new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(absoluteExpiredTime)),
                    getLatestData);

        public async Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool getLatestData = false)
            => await cacheService.GetOrSetAsync(key, factory, options).ConfigureAwait(false);

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

        public async Task InvalidateAll()
        {
            foreach (var key in cachedKeys)
            {
                await cacheService.RemoveAsync(key);
            }

            cachedKeys.Clear();
        }
    }
}