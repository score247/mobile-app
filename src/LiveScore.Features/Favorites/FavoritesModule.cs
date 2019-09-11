﻿namespace LiveScore.Features.Favorites
{
    using ViewModels;
    using Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class FavoritesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // On initialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FavoriteView, FavoriteViewModel>();
            containerRegistry.RegisterForNavigation<EmptyFavoriteView, EmptyFavoriteViewModel>();
        }
    }
}