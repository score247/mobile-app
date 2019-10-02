namespace LiveScore.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
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
        private readonly INetworkConnectionManager networkConnectionManager;

        public CacheManager(
            ICacheService cacheService, 
            INetworkConnectionManager networkConnectionManager)
        {
            this.cacheService = cacheService;
            this.networkConnectionManager = networkConnectionManager;
            cachedKeys = new List<string>();
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false)
        {
            if (networkConnectionManager.IsConnectionOK())
            {
                if (getLatestData)
                {
                    try
                    {
                        var data = await factory.Invoke().ConfigureAwait(false);

                        await SetCacheAsync(key, data, options).ConfigureAwait(false);
                    }
                    catch(Exception ex)
                    {
                        if (ex is TaskCanceledException)
                        {
                            networkConnectionManager.PublishConnectionTimeoutEvent();
                        }

                        throw;
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
                    catch (Exception ex)
                    {
                        if (ex is TaskCanceledException)
                        {
                            networkConnectionManager.PublishConnectionTimeoutEvent();
                        }

                        throw;
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