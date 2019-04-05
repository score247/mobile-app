namespace LiveScore.News
{
    using LiveScore.News.ViewModels;
    using LiveScore.News.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class NewsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NewsView, NewsViewModel>();
        }
    }
}