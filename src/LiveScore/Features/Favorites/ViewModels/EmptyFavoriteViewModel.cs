namespace LiveScore.Features.Favorites.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyFavoriteViewModel : ViewModelBase
    {
        public EmptyFavoriteViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}