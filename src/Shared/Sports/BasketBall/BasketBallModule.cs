namespace LiveScore.Basketball
{
    using LiveScore.Basketball.Services;
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Xamarin.Forms;

    public class BasketballModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMatchService, BasketballMatchService>(SportType.Basketball.GetDescription());
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportType.Basketball.GetDescription());
        }
    }
}
