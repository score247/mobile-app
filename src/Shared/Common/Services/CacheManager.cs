namespace LiveScore.Common.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Caching;

    public enum CacheDuration
    {
        Short = 120, // 120 seconds
        Long = 7200
    }

    public interface ICacheManager
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool getLatestData = false);

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool getLatestData = false);
    }

    public class CacheManager : ICacheManager
    {
        private readonly ICacheService cacheService;

        public CacheManager(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        // TODO: refactor later
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool getLatestData = false)
        {
            if (getLatestData)
            {
                var data = await factory.Invoke().ConfigureAwait(false);

                await cacheService.SetAsync(key, data, options).ConfigureAwait(false);
            }

            var dataFromCache = await cacheService.GetAsync<T>(key).ConfigureAwait(false);

            if (Equals(dataFromCache, default(T)))
            {
                var data = await factory.Invoke().ConfigureAwait(false);

                await cacheService.SetAsync(key, data, options).ConfigureAwait(false);
            }

            return await cacheService.GetAsync<T>(key).ConfigureAwait(false);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int absoluteExpiredTime, bool getLatestData = false)
            => GetOrSetAsync(key, factory,
                new CacheItemOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(absoluteExpiredTime)),
                    getLatestData);

        public async Task<T> GetOrSetAsync<T>(string key, Func<T> factory, CacheItemOptions options, bool getLatestData = false)
            => await cacheService.GetOrSetAsync(key, factory, options).ConfigureAwait(false);
    }
}