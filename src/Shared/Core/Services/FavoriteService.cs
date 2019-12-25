﻿using System;
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
        protected readonly IEventAggregator eventAggregator;

        protected string Key;
        protected int Limitation;
        protected IList<T> Objects;

        protected FavoriteService(IUserSettingService userSettingService, IEventAggregator eventAggregator)
        {
            this.userSettingService = userSettingService;
            this.eventAggregator = eventAggregator;
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

        public virtual IList<T> GetAll() => Objects;

        public virtual void Add(T obj)
        {
            if (Objects.Count > Limitation)
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