namespace Favorites.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}