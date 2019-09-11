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

    public interface ICachingService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool forceFetchNew = false);
    }

    public class CachingService : ICachingService
    {
        private readonly ICacheService cacheService;

        public CachingService(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheItemOptions options, bool forceFetchNew = false)
        {
            if (forceFetchNew)
            {
                await cacheService.RemoveAsync(key);
            }

            var data = await factory.Invoke();

            if (data == default)
            {
                data = await cacheService.GetAsync<T>(key);
            }
            else
            {
                await cacheService.SetAsync(key, data, options);
            }

            return data;
        }
    }
}