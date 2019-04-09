namespace LiveScore.BasketBall
{
    using LiveScore.BasketBall.Factories;
    using LiveScore.Core.Factories;
    using Prism.Ioc;
    using Prism.Modularity;

    public class BasketBallModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var globalServiceProvider = containerProvider.Resolve<IGlobalFactoryProvider>();
            var basketBallServiceFactory = new BasketBallServiceFactory();
            basketBallServiceFactory.RegisterTo(globalServiceProvider.SportServiceFactoryProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //
        }
    }
}
