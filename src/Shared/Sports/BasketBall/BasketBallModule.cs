namespace LiveScore.Basketball
{
    using LiveScore.Basketball.Services;
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Services;
    using LiveScore.Core.Views.Selectors;
    using Prism.Ioc;
    using Prism.Modularity;

    public class BasketballModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMatchService, MatchService>(SportType.Basketball.GetDescription());
            containerRegistry.Register<MatchItemTemplate, MatchDataTemplate>(SportType.Basketball.GetDescription());
        }
    }
}
