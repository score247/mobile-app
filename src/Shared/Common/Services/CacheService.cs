namespace LiveScore.Common.Services
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Akavache;

    public interface ICacheService
    {
        Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null);

        Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, bool forceFetch = false, DateTime? absoluteExpiration = null);

        Task SetValue<T>(string name, T value);

        void Shutdown();
    }

    public class CacheService : ICacheService
    {
        const string STORAGE_NAME = "LiveScore.Storage";
        const DateTimeKind DEFAULT_DATETIMEKIND = DateTimeKind.Local;

        public CacheService() 
        {
            Registrations.Start(STORAGE_NAME);
            BlobCache.ForcedDateTimeKind = DEFAULT_DATETIMEKIND;
        }

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

        public void Shutdown() => BlobCache.Shutdown().Wait();
    }
}
