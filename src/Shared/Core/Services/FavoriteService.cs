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

        void Add(T obj);

        void Remove(T obj);

        bool IsFavorite(T obj);
    }

    public abstract class FavoriteService<T> : IFavoriteService<T>
    {
        protected readonly IUserSettingService userSettingService;
        protected readonly IEventAggregator eventAggrerator;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;

        public FavoriteService(IUserSettingService userSettingService, IEventAggregator eventAggrerator)
        {
            this.userSettingService = userSettingService;
            this.eventAggrerator = eventAggrerator;
        }

        protected Func<Task> OnAddedFunc { get; set; }

        protected Func<T, Task> OnRemovedFunc { get; set; }

        protected Func<Task> OnReachedLimit { get; set; }

        public void Init(string key, int limitation)
        {
            Key = key;
            Limitation = limitation;

            Objects = LoadCache();
        }

        public IList<T> GetAll() => Objects;

        public virtual void Add(T obj)
        {
            if (Objects.Count >= Limitation)
            {
                OnReachedLimit?.Invoke();
                return;
            }

            if (!Objects.Contains(obj))
            {
                Objects.Add(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
        }

        public void Remove(T obj)
        {
            if (Objects.Contains(obj))
            {
                Objects.Remove(obj);
            }

            Task.Run(UpdateCache).ConfigureAwait(false);

            OnRemovedFunc?.Invoke(obj);
        }

        public bool IsFavorite(T obj) => Objects.Contains(obj);

        public abstract void UpdateCache();

        public abstract IList<T> LoadCache();
    }
}