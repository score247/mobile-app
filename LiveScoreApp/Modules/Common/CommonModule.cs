namespace Common
{
    using System;
    using System.Net.Http;
    using Common.Helpers;
    using Common.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;

    public class CommonModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(
                RestService.For<IMatchApi>(
                    new HttpClient(new HttpLoggingHandler())
                    {
                        BaseAddress = new Uri(Settings.Settings.BaseSportRadarEndPoint)
                    }));
            containerRegistry.Register<IMatchService, MatchService>();
        }
    }
}