﻿// <auto-generated>
// Not implemented yet
// </auto-generated>
namespace LiveScore.Features.News
{
    using Prism.Ioc;
    using Prism.Modularity;
    using ViewModels;
    using Views;

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