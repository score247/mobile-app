using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Caching;
using MethodTimer;

namespace LiveScore.Common.Services
{
    public interface ICacheManager
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool forceFetchLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool forceFetchLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool forceFetchLatestData = false);

        Task InvalidateAllAsync();
    }

    public class CacheManager : ICacheManager
    {
        private readonly ICacheService fanexCacheService;
        private readonly ILoggingService loggingService;
        private readonly List<string> cachedKeys;

        public CacheManager(ICacheService fanexCacheService, ILoggingService loggingService)
        {
            this.fanexCacheService = fanexCacheService;
            this.loggingService = loggingService;

            //TODO : thread-safe???
            cachedKeys = new List<string>();
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(
            string key,
            Func<Task<T>> factory,
            CacheItemOptions options,
            bool forceFetchLatestData = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return default;
                }

                if (forceFetchLatestData)
                {
                    await GetFromRemoteApiAndSetToCacheAsync(key, factory, options);
                }

                var dataFromCache = await fanexCacheService.GetAsync<T>(key).ConfigureAwait(false);

                if (Equals(dataFromCache, default(T)))
                {
                    await GetFromRemoteApiAndSetToCacheAsync(key, factory, options);
                }
            }
            catch (TaskCanceledException ex)
            {
                await loggingService.LogExceptionAsync(ex);
            }

            return await fanexCacheService.GetAsync<T>(key).ConfigureAwait(false);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool forceFetchLatestData = false)
            => GetOrSetAsync(
                key,
                factory,
                new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(absoluteExpiredTime)),
                forceFetchLatestData);

        public async Task<T> GetOrSetAsync<T>(
            string key,
            Func<T> factory,
            CacheItemOptions options,
            bool forceFetchLatestData = false)
            =>
            await fanexCacheService.GetOrSetAsync(key, factory, options).ConfigureAwait(false);

        public async Task InvalidateAllAsync()
        {
            foreach (var key in cachedKeys)
            {
                await fanexCacheService.RemoveAsync(key).ConfigureAwait(false);
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

            return fanexCacheService.SetAsync(key, data, options);
        }

        private async Task GetFromRemoteApiAndSetToCacheAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options)
        {
            var data = await factory.Invoke().ConfigureAwait(false);
            await SetCacheAsync(key, data, options).ConfigureAwait(false);
        }
    }
}