using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Favorites;
using Prism.Events;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService<T>
    {
        IList<T> GetAll();

        bool Add(T obj);

        void Remove(T obj);

        bool IsFavorite(T obj);

        Task Sync(IList<Favorite> addedFavorites = null, IList<Favorite> removedFavorites = null);
    }

    public abstract class FavoriteService<T> : BaseService, IFavoriteService<T>
    {
        private const string AddedFavoritesCacheKey = "AddedFavorites";
        private const string RemovedFavoritesCacheKey = "RemovedFavorites";
        protected readonly IUserSettingService userSettingService;
        protected readonly IEventAggregator eventAggregator;
        protected readonly INetworkConnection NetworkConnection;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;
        protected List<Favorite> AddedFavorites;
        protected List<Favorite> RemovedFavorites;

        protected FavoriteService(
            IUserSettingService userSettingService,
            IEventAggregator eventAggregator,
            string key,
            int limitation,
            ILoggingService loggingService,
            INetworkConnection networkConnection) : base(loggingService)
        {
            this.userSettingService = userSettingService;
            this.eventAggregator = eventAggregator;
            NetworkConnection = networkConnection;
            Key = key;
            Limitation = limitation;
        }

        protected Func<T, Task> OnAddedFunc { get; set; }

        protected Func<T, Task> OnRemovedFunc { get; set; }

        protected Func<Task> OnReachedLimitFunc { get; set; }

        protected Func<IList<Favorite>, IList<Favorite>, Task<bool>> SyncFunc { get; set; }

        public void Init()
        {
            Objects = LoadCache();
            AddedFavorites = userSettingService.GetValueOrDefault(AddedFavoritesCacheKey + Key, new List<Favorite>());
            RemovedFavorites = userSettingService.GetValueOrDefault(RemovedFavoritesCacheKey + Key, new List<Favorite>());
        }

        public virtual IList<T> GetAll() => Objects;

        public virtual bool Add(T obj)
        {
            if (Objects.Count >= Limitation)
            {
                OnReachedLimitFunc?.Invoke();
                return false;
            }

            if (!Objects.Contains(obj))
            {
                Objects.Add(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);

            OnAddedFunc?.Invoke(obj);
            return true;
        }

        public void Remove(T obj)
        {
            RemoveFromCache(obj);

            OnRemovedFunc?.Invoke(obj);
        }

        public virtual bool IsFavorite(T obj) => Objects.Contains(obj);

        public abstract void UpdateCache();

        public abstract IList<T> LoadCache();

        protected void RemoveFromCache(T obj)
        {
            if (Objects.Contains(obj))
            {
                Objects.Remove(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);
        }

        public async Task Sync(IList<Favorite> addedFavorites = null, IList<Favorite> removedFavorites = null)
        {
            AddedFavorites.AddRange(addedFavorites ?? new List<Favorite>());
            RemovedFavorites.AddRange(removedFavorites ?? new List<Favorite>());

            if (NetworkConnection.IsSuccessfulConnection())
            {
                var syncSuccessful = await SyncFunc?.Invoke(AddedFavorites, RemovedFavorites);

                if (syncSuccessful)
                {
                    AddedFavorites.Clear();
                    RemovedFavorites.Clear();
                }
            }

            userSettingService.AddOrUpdateValue(AddedFavoritesCacheKey + Key, AddedFavorites);
            userSettingService.AddOrUpdateValue(RemovedFavoritesCacheKey + Key, RemovedFavorites);
        }
    }
}