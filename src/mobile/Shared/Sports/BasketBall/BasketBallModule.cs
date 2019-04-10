namespace LiveScore.Basketball
{
    using LiveScore.Basketball.Factories;
    using LiveScore.Core.Factories;
    using Prism.Ioc;
    using Prism.Modularity;

    public class BasketballModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var globalServiceProvider = containerProvider.Resolve<IGlobalFactoryProvider>();
            var basketBallServiceFactory = new BasketballServiceFactory();
            basketBallServiceFactory.RegisterTo(globalServiceProvider.ServiceFactoryProvider);

            var basketballTemplateFactory = new BasketballTemplateFactory();
            basketballTemplateFactory.RegisterTo(globalServiceProvider.TemplateFactoryProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
