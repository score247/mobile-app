namespace LiveScore.Favorites.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class FavoriteViewModel : ViewModelBase
    {
        public FavoriteViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}