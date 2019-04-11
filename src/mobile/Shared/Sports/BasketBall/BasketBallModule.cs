namespace LiveScore.Basketball
{
    using LiveScore.Basketball.Factories;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using Prism.Ioc;
    using Prism.Modularity;

    public class BasketballModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var globalServiceProvider = containerProvider.Resolve<IGlobalFactoryProvider>();
            var basketBallServiceFactory = new BasketballServiceFactory();
            globalServiceProvider.ServiceFactoryProvider.RegisterInstance(SportType.Basketball, basketBallServiceFactory);

            var basketballTemplateFactory = new BasketballTemplateFactory();
            globalServiceProvider.TemplateFactoryProvider.RegisterInstance(SportType.Basketball, basketballTemplateFactory);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
