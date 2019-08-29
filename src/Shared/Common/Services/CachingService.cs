namespace LiveScore.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Akavache;
    using MethodTimer;

    public enum CacheDuration
    {
        Short = 120, // 120 seconds
        Long = 7200
    }

    public interface ICachingService
    {
        Task<T> GetOrFetchLocalMachine<T>(string key, Func<Task<T>> fetchFunc, DateTimeOffset? absoluteExpiration = null);

        Task<T> GetAndFetchLatestLocalMachine<T>(
            string key, Func<Task<T>> fetchFunc, Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null);

        Task<T> GetOrCreateLocalMachine<T>(string key, T defaultValue);

        Task InsertLocalMachine<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);

        Task<T> GetOrCreateInMemory<T>(string key, T defaultValue);

        Task InsertInMemory<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);

        T GetOrCreateUserAccount<T>(string key, T defaultValue);

        void InsertUserAccount<T>(string key, T value);

        Task InvalidateLocalMachine(string key);

        Task InvalidateLocalMachine(IEnumerable<string> keys);

        Task VacuumLocalMachine();

        Func<DateTimeOffset, bool> GetFetchPredicate(bool forceFecth, int seconds);

        Task InvalidateAll();

        void FlushAll();
    }

    public class CachingService : ICachingService
    {
        private readonly IBlobCache localMachineCache;
        private readonly IBlobCache userAccountCache;
        private readonly IBlobCache inMemoryCache;

        [Time]
        public CachingService(IEssential essential, IBlobCache localMachine = null, IBlobCache userAccount = null, IBlobCache inMemory = null)
        {
            if (essential == null)
            {
                throw new ArgumentNullException(nameof(essential));
            }

            localMachineCache = localMachine ?? BlobCache.LocalMachine;
            userAccountCache = userAccount ?? BlobCache.UserAccount;
            inMemoryCache = inMemory ?? BlobCache.InMemory;

            BlobCache.ForcedDateTimeKind = DateTimeKind.Local;
        }

        [Time]
        public async Task<T> GetOrFetchLocalMachine<T>(
            string key,
            Func<Task<T>> fetchFunc,
            DateTimeOffset? absoluteExpiration = null)
                => await localMachineCache.GetOrFetchObject(key, fetchFunc, absoluteExpiration);

        [Time]
        public async Task<T> GetAndFetchLatestLocalMachine<T>(
            string key,
            Func<Task<T>> fetchFunc,
            Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null)
                => await localMachineCache.GetAndFetchLatest(key, fetchFunc, fetchPredicate, absoluteExpiration);

        public async Task<T> GetOrCreateLocalMachine<T>(string key, T defaultValue)
            => await localMachineCache.GetOrCreateObject(key, () => defaultValue);

        public async Task InsertLocalMachine<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
            => await localMachineCache.InsertObject(key, value, absoluteExpiration);

        public async Task<T> GetOrCreateInMemory<T>(string key, T defaultValue)
          => await inMemoryCache.GetOrCreateObject(key, () => defaultValue);

        public async Task InsertInMemory<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
            => await inMemoryCache.InsertObject(key, value, absoluteExpiration);

        public T GetOrCreateUserAccount<T>(string key, T defaultValue)
            => userAccountCache.GetOrCreateObject(key, () => defaultValue).Wait();

        public void InsertUserAccount<T>(string key, T value)
            => userAccountCache.InsertObject(key, value).Wait();

        public async Task InvalidateLocalMachine(string key) => await localMachineCache.Invalidate(key);

        public async Task InvalidateLocalMachine(IEnumerable<string> keys) => await localMachineCache.Invalidate(keys);

        public async Task VacuumLocalMachine() => await localMachineCache.Vacuum();

        public Func<DateTimeOffset, bool> GetFetchPredicate(bool forceFecth, int seconds)
        {
            if (forceFecth)
            {
                return _ => forceFecth;
            }

            return (offset) =>
            {
                var elapsed = DateTimeOffset.Now - offset;

                return elapsed > new TimeSpan(0, 0, seconds);
            };
        }

        public async Task InvalidateAll()
        {
            await inMemoryCache.InvalidateAll();
            await userAccountCache.InvalidateAll();
            await localMachineCache.InvalidateAll();
        }

        public void FlushAll()
        {
            inMemoryCache.Flush().Wait();
            userAccountCache.Flush().Wait();
            localMachineCache.Flush().Wait();
        }
    }
}