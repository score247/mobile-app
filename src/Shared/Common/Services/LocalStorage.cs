namespace LiveScore.Common.Services
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Akavache;
    using Splat;

    public enum CacheDurationTerm
    {
        Short,
        Long
    }

    public interface ILocalStorage
    {
        Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null);

        Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, Func<DateTimeOffset, bool> fetchPredicate = null, DateTimeOffset? absoluteExpiration = null);

        void Shutdown();

        Task Invalidate(string key);

        Task<IBitmap> LoadImageFromUrl(string imageLink, float? desiredWidth = null, float? desiredHeight = null);

        TimeSpan CacheDuration(CacheDurationTerm cacheKind);

        Task CleanAllExpired();

        void AddOrUpdateValue<T>(string key, T value);

        T GetValueOrDefault<T>(string key, T defaultValue);

        void InsertValue<T>(string name, T value, DateTimeOffset? absoluteExpiration = null);
    }

    public class LocalStorage : ILocalStorage
    {
        private const DateTimeKind DEFAULT_DATETIMEKIND = DateTimeKind.Local;
        private const int ShortTerm = 30;
        private const int LongTerm = 7200;

        private readonly IBlobCache LocalMachine;
        private readonly IBlobCache UserAccount;

        public LocalStorage(IEssentialsService essentials, IBlobCache localMachine = null, IBlobCache userAccount = null)
        {
            Registrations.Start(essentials.AppName);

            LocalMachine = localMachine ?? BlobCache.LocalMachine;
            LocalMachine.ForcedDateTimeKind = DEFAULT_DATETIMEKIND;

            UserAccount = userAccount ?? BlobCache.UserAccount;
        }

        public async Task<T> GetOrFetchValue<T>(string name, Func<Task<T>> fetchFunc, DateTime? absoluteExpiration = null)
            => await LocalMachine.GetOrFetchObject(name, fetchFunc, absoluteExpiration);

        public async Task<T> GetAndFetchLatestValue<T>(string name, Func<Task<T>> fetchFunc, Func<DateTimeOffset, bool> fetchPredicate = null, DateTimeOffset? absoluteExpiration = null)
            => await LocalMachine.GetAndFetchLatest(name, fetchFunc, fetchPredicate, absoluteExpiration);

        public async void InsertValue<T>(string name, T value, DateTimeOffset? absoluteExpiration = null)
            => await LocalMachine.InsertObject(name, value, absoluteExpiration);

        public void Shutdown()
        {
            UserAccount.Flush().Wait();
            LocalMachine.Flush().Wait();
        }

        public async Task Invalidate(string key) => await LocalMachine.Invalidate(key);

        public async Task<IBitmap> LoadImageFromUrl(string imageLink, float? desiredWidth = null, float? desiredHeight = null)
            => await LocalMachine.LoadImageFromUrl(imageLink, desiredWidth: desiredWidth, desiredHeight: desiredHeight);

        public TimeSpan CacheDuration(CacheDurationTerm cacheKind)
            => cacheKind == CacheDurationTerm.Short
                ? new TimeSpan(0, 0, ShortTerm)
                : new TimeSpan(0, 0, LongTerm);

        public async Task CleanAllExpired() => await LocalMachine.Vacuum();

        public void AddOrUpdateValue<T>(string key, T value)
            => UserAccount.InsertObject(key, value).Wait();

        public T GetValueOrDefault<T>(string key, T defaultValue)
            => UserAccount.GetOrCreateObject(key, () => defaultValue).Wait();
    }
}