namespace LiveScore.Core
{
    using Prism.Ioc;

    public interface IApplicationContext
    {
        int SportId { get; set; }
    }

    public interface IDepdendencyResolver
    {
        T Resolve<T>();

        T Resolve<T>(string name);
    }

    public class DepdendencyResolver : IDepdendencyResolver
    {
        private readonly IContainerProvider containerProvider;

        public DepdendencyResolver(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public T Resolve<T>() => containerProvider.Resolve<T>();

        public T Resolve<T>(string name) => containerProvider.Resolve<T>(name);
    }
}