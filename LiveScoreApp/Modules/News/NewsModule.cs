namespace News
{
    using News.ViewModels;
    using News.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class NewsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NewsPage, NewsPageViewModel>();
        }
    }
}