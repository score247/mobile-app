﻿namespace LiveScore.Common.Services
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

        private IList<string> cachedKeys;

        public CacheManager(ICacheService cacheService)
        {
            this.cacheService = cacheService;
            cachedKeys = new List<string>();
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false)
        {
            if (getLatestData)
            {
                var data = await factory.Invoke().ConfigureAwait(false);

                await SetCacheAsync(key, data, options).ConfigureAwait(false);
            }

            var dataFromCache = await cacheService.GetAsync<T>(key).ConfigureAwait(false);

            if (Equals(dataFromCache, default(T)))
            {
                var data = await factory.Invoke().ConfigureAwait(false);

                await SetCacheAsync(key, data, options).ConfigureAwait(false);
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