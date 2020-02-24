using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Views;
using Rg.Plugins.Popup.Contracts;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService<T>
    {
        Func<T, Task> OnAddedFavorite { get; set; }

        Func<T, Task> OnRemovedFavorite { get; set; }

        Func<Task> OnReachLimitFavoriteItems { get; set; }

        IList<T> GetAll();

        bool Add(T obj);

        void Remove(T obj);

        bool IsFavorite(T obj);

        Task Sync(IList<Favorite> addedFavorites = null, IList<Favorite> removedFavorites = null);
    }

    public abstract class FavoriteService<T> : BaseService, IFavoriteService<T>
    {
        protected readonly IUserSettingService userSettingService;
        protected readonly INetworkConnection NetworkConnection;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;
        protected List<Favorite> AddedFavorites;
        protected List<Favorite> RemovedFavorites;

        private const string AddedFavoritesCacheKey = "AddedFavorites";
        private const string RemovedFavoritesCacheKey = "RemovedFavorites";
        private readonly IPopupNavigation popupNavigation;

        protected FavoriteService(
            string key,
            int limitation,
            IDependencyResolver dependencyResolver) : base(dependencyResolver.Resolve<ILoggingService>())
        {
            Key = key;
            Limitation = limitation;
            userSettingService = dependencyResolver.Resolve<IUserSettingService>();
            NetworkConnection = dependencyResolver.Resolve<INetworkConnection>();
            popupNavigation = dependencyResolver.Resolve<IPopupNavigation>();
        }

        public Func<T, Task> OnAddedFavorite { get; set; }

        public Func<T, Task> OnRemovedFavorite { get; set; }

        public Func<Task> OnReachLimitFavoriteItems { get; set; }

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
                popupNavigation.PushAsync(
                    new FavoritePopupView(string.Format(AppResources.FavoriteLimitation, Limitation, Key)));
                OnReachLimitFavoriteItems?.Invoke();
                return false;
            }

            if (!Objects.Contains(obj))
            {
                Objects.Add(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);

            popupNavigation.PushAsync(new FavoritePopupView(AppResources.AddedFavorite));
            OnAddedFavorite?.Invoke(obj);

            return true;
        }

        public void Remove(T obj)
        {
            RemoveFromCache(obj);

            popupNavigation.PushAsync(new FavoritePopupView(AppResources.RemovedFavorite));
            OnRemovedFavorite?.Invoke(obj);
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

            if (AddedFavorites.Count == 0 && RemovedFavorites.Count == 0)
            {
                return;
            }

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