namespace LiveScore.Soccer
{
    using LiveScore.Common.Configuration;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.Views.Templates;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;
    using Xamarin.Forms;

    public class SoccerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(RestService.For<ISoccerMatchApi>(Configuration.LocalEndPoint));
            containerRegistry.RegisterInstance(RestService.For<ILeagueApi>(Configuration.LocalEndPoint));
            containerRegistry.Register<IMatchService, MatchService>(SportType.Soccer.GetDescription());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportType.Soccer.GetDescription());
        }
    }
}