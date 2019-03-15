namespace Favorites
{
    using Favorites.ViewModels;
    using Favorites.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class FavoritesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FavoritePage, FavoritePageViewModel>();
        }
    }
}