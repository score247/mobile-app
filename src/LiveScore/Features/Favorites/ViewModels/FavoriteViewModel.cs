namespace LiveScore.Features.Favorites.ViewModels
{
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;

    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}