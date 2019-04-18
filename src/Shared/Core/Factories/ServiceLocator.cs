namespace LiveScore.Core.Factories
{
    using Prism.Ioc;

    public interface IApplicationContext
    {
        int SportId { get; set; }
    }

    public interface IServiceLocator
    {
        T Create<T>();

        T Create<T>(string name);
    }

    public class ServiceLocator : IServiceLocator
    {
        private readonly IContainerProvider containerProvider;

        public ServiceLocator(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public T Create<T>() => containerProvider.Resolve<T>();

        public T Create<T>(string name) => containerProvider.Resolve<T>(name);
    }
}
