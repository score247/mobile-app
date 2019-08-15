﻿namespace LiveScore.Common.Services
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Akavache;

    public enum CacheDuration
    {
        Short = 120, // 120 seconds
        Long = 7200
    }

    public interface ICachingService
    {
        Task<T> GetOrFetchValue<T>(string key, Func<Task<T>> fetchFunc, DateTimeOffset? absoluteExpiration = null);

        Task<T> GetAndFetchLatestValue<T>(
            string key, Func<Task<T>> fetchFunc, Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null);

        Task InsertValue<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);

        Task<T> GetValueOrDefault<T>(string key, T defaultValue);

        Task InsertValueInMemory<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);

        Task<T> GetValueOrDefaultInMemory<T>(string key, T defaultValue);

        void AddOrUpdateValueToUserAccount<T>(string key, T value);

        T GetValueOrDefaultFromUserAccount<T>(string key, T defaultValue);

        void Shutdown();

        Task Invalidate(string key);

        Task CleanAllExpired();

        Func<DateTimeOffset, bool> GetFetchPredicate(bool forceFecth, int seconds);
    }

    public class CachingService : ICachingService
    {
        private readonly IBlobCache localMachineCache;
        private readonly IBlobCache userAccountCache;
        private readonly IBlobCache inMemoryCache;

        public CachingService(IEssential essential, IBlobCache localMachine = null, IBlobCache userAccount = null, IBlobCache inMemory = null)
        {
            if (essential ==  null)
            {
                throw new ArgumentNullException(nameof(essential));
            }

            
            localMachineCache = localMachine ?? BlobCache.LocalMachine;
            localMachineCache.ForcedDateTimeKind = DateTimeKind.Local;
            userAccountCache = userAccount ?? BlobCache.UserAccount;
            inMemoryCache = inMemory ?? BlobCache.InMemory;
        }

        public async Task<T> GetOrFetchValue<T>(
            string key,
            Func<Task<T>> fetchFunc,
            DateTimeOffset? absoluteExpiration = null)
            => await localMachineCache.GetOrFetchObject(key, fetchFunc, absoluteExpiration);

        public async Task<T> GetAndFetchLatestValue<T>(
            string key, Func<Task<T>> fetchFunc, Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null)
            => await localMachineCache.GetAndFetchLatest(key, fetchFunc, fetchPredicate, absoluteExpiration);

        public async Task<T> GetValueOrDefault<T>(string key,
            T defaultValue)
            => await localMachineCache.GetOrCreateObject(key, () => defaultValue);

        public async Task InsertValue<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
            => await localMachineCache.InsertObject(key, value, absoluteExpiration);

        public async Task<T> GetValueOrDefaultInMemory<T>(string key, T defaultValue)
          => await inMemoryCache.GetOrCreateObject(key, () => defaultValue);

        public async Task InsertValueInMemory<T>(string key, T value, DateTimeOffset? absoluteExpiration = null)
            => await inMemoryCache.InsertObject(key, value, absoluteExpiration);

        public void Shutdown()
        {
            userAccountCache.Flush().Wait();
            localMachineCache.Flush().Wait();
        }

        public async Task Invalidate(string key) => await localMachineCache.Invalidate(key);

        public async Task CleanAllExpired() => await localMachineCache.Vacuum();

        public void AddOrUpdateValueToUserAccount<T>(string key, T value)
            => userAccountCache.InsertObject(key, value).Wait();

        public T GetValueOrDefaultFromUserAccount<T>(string key, T defaultValue)
            => userAccountCache.GetOrCreateObject(key, () => defaultValue).Wait();

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
    }
}