namespace LiveScore.Favorites.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyFavoriteViewModel : ViewModelBase
    {
        public EmptyFavoriteViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
        }
    }
}