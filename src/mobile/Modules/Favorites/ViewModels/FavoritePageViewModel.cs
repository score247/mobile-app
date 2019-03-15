namespace Favorites.ViewModels
{
    using Common.ViewModels;
    using Prism.Navigation;

    public class FavoritePageViewModel : ViewModelBase
    {
        public FavoritePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}