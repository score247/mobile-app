using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public abstract class FavoriteService<T> : IFavoriteService<T> 
    {
        protected readonly IUserSettingService userSettingService;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;

        public FavoriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;
        }

        public Func<Task> OnAddedFunc { get; set; }

        public Func<T, Task> OnRemovedFunc { get; set; }

        public Func<Task> OnReachedLimit { get; set; }

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
