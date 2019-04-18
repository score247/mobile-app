namespace LiveScore.Common.Services
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Akavache;
    using Splat;

    public interface ICacheService
    {
        Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null);

        Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, bool forceFetch = false, DateTime? absoluteExpiration = null);

        Task SetValue<T>(string name, T value);

        void Shutdown();

        Task Invalidate(string key);

        Task<IBitmap> LoadImageFromUrl(string url, float? desiredWidth = null, float? desiredHeight = null);

        DateTime CacheDuration(CacheDurationKind cacheKind);
    }

    public enum CacheDurationKind
    {
        Short,
        Long
    }

    public class CacheService : ICacheService
    {
        const DateTimeKind DEFAULT_DATETIMEKIND = DateTimeKind.Local;
        const int ShortTerm = 2;
        const int LongTerm = 120;

        public CacheService(IEssentialsService essentials)
        {
            Registrations.Start(essentials.AppName);
            BlobCache.ForcedDateTimeKind = DEFAULT_DATETIMEKIND;
        }

        public async Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null)
        => await BlobCache.LocalMachine.GetOrFetchObject(name, fetchFunc, absoluteExpiration);


        public async Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, bool forceFetch = false, DateTime? absoluteExpiration = null)
        => await BlobCache.LocalMachine.GetAndFetchLatest(name, fetchFunc, (offset) => forceFetch, absoluteExpiration);


        public async Task SetValue<T>(string name, T value)
        => await BlobCache.LocalMachine.InsertObject(name, value);

        public void Shutdown()
        {
            BlobCache.UserAccount.Flush().Wait();
            BlobCache.LocalMachine.Flush().Wait();
        }

        public async Task Invalidate(string key) => await BlobCache.LocalMachine.Invalidate(key);

        public async Task<IBitmap> LoadImageFromUrl(string url, float? desiredWidth = null, float? desiredHeight = null) 
            => await BlobCache.LocalMachine.LoadImageFromUrl(url, false, desiredWidth, desiredHeight);

        public DateTime CacheDuration(CacheDurationKind cacheKind)
        => cacheKind == CacheDurationKind.Short 
            ? DateTime.Now.AddMinutes(ShortTerm) 
            : DateTime.Now.AddMinutes(LongTerm);
    }
}
