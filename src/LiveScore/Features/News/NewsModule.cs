namespace LiveScore.Features.News
{
    using LiveScore.Features.News.ViewModels;
    using LiveScore.Features.News.Views;
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
            containerRegistry.RegisterForNavigation<EmptyNewsView, EmptyNewsViewModel>();
        }
    }
}