using System;
using Prism.Ioc;

namespace LiveScore.Core
{
    public interface IDependencyResolver
    {
        T Resolve<T>();

        T Resolve<T>(string name);
    }

    public class DependencyResolver : IDependencyResolver
    {
        private readonly IContainerProvider containerProvider;

        public DependencyResolver(IContainerProvider containerProvider)
        {
            this.containerProvider
                = containerProvider ?? throw new ArgumentNullException(nameof(containerProvider));
        }

        public T Resolve<T>() => containerProvider.Resolve<T>();

        public T Resolve<T>(string name) => containerProvider.Resolve<T>(name);
    }
}