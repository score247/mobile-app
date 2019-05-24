namespace LiveScore.Basketball
{
    using LiveScore.Basketball.Services;
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Core.Enumerations;
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
            containerRegistry.Register<IMatchService, MatchService>(SportTypes.Basketball.Value);
            containerRegistry.Register<DataTemplate, MatchDataTemplate>(SportTypes.Basketball.Value);
        }
    }
}
