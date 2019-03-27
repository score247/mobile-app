namespace Common.Services
{
    using System;
    using System.Threading.Tasks;
    using Akavache;
    using System.Reactive.Linq;

    public interface ICacheService
    {
        Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null);

        Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, bool forceFetch = false, DateTime? absoluteExpiration = null);

        Task SetValue<T>(string name, T value);

    }

    public class CacheService : ICacheService
    {
        public async Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null)
        {
            return await BlobCache.LocalMachine.GetOrFetchObject(name, fetchFunc, absoluteExpiration);
        }

        public async Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, bool forceFetch = false, DateTime? absoluteExpiration = null)
        {
            return await BlobCache.LocalMachine.GetAndFetchLatest(name, fetchFunc, (offset) => forceFetch, absoluteExpiration);
        }

        public async Task SetValue<T>(string name, T value)
        {
            await BlobCache.LocalMachine.InsertObject(name, value);
        }
    }
}
