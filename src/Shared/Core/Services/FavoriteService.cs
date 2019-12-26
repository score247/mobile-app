using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using Prism.Events;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService<T>
    {
        IList<T> GetAll();

        bool Add(T obj);

        void Remove(T obj);

        bool IsFavorite(T obj);
    }

    public abstract class FavoriteService<T> : IFavoriteService<T>
    {
        protected readonly IUserSettingService userSettingService;
        protected readonly IEventAggregator eventAggregator;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;

        protected FavoriteService(IUserSettingService userSettingService, IEventAggregator eventAggregator, string key, int limitation)
        {
            this.userSettingService = userSettingService;
            this.eventAggregator = eventAggregator;
            Key = key;
            Limitation = limitation;
        }

        protected Func<Task> OnAddedFunc { get; set; }

        protected Func<T, Task> OnRemovedFunc { get; set; }

        protected Func<Task> OnReachedLimit { get; set; }

        public void Init()
        {
            Objects = LoadCache();
        }

        public virtual IList<T> GetAll() => Objects;

        public virtual bool Add(T obj)
        {
            if (Objects.Count >= Limitation)
            {
                OnReachedLimit?.Invoke();
                return false;
            }

            if (!Objects.Contains(obj))
            {
                Objects.Add(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
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
    }
}