using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService<T>
    {
        IList<T> GetAll();

        void Add(T obj);

        void Remove(T obj);

        bool IsFavorite(T obj);

        Func<Task> OnAddedFunc { get; set; }

        Func<T, Task> OnRemovedFunc { get; set; }

        Func<Task> OnReachedLimit { get; set; }
    }
}